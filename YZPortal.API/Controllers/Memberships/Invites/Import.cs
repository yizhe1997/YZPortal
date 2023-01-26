using AutoMapper;
using ExcelDataReader;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.RegularExpressions;


namespace YZPortal.API.Controllers.Memberships.Invites
{
    public class Import
    {
        private static readonly string enableB2C = "Yes";

        public class Request : IRequest<Model>
        {
            [Required]
            internal IFormFile file { get; set; }
            // example: https://dealerportalfrontend.z13.web.core.windows.net/redeem/{0}
            [Required]
            public string CallbackUrl { get; set; }
            public bool TurnOffRegExForEmailValidation { get; set; }
            public bool TurnOffEmailRequireDotInDomainName { get; set; }
        }
        public class Sheet
        {
            internal bool HasError { get; set; } = false;
            public int Index { get; set; }
            public string Email { get; set; }
            public string Name { get; set; }
            public string B2C { get; set; }
            public string Dealership { get; set; }
            public string UserRoles { get; set; }
            public string UserAccessLevels { get; set; }
            internal string CallbackUrl { get; set; }
            // Added empty just for the no null field validation
            public string ErrorMsg { get; set; } = InvitesHelper.emptyErrorMsg;
            internal bool IgnoreExistingMembershipCheck { get; set; }
        }

        public class Model
        {
            public List<InviteViewModel> Invites { get; set; }
        }

        /// <summary>
        ///  ref.: https://html.spec.whatwg.org/multipage/forms.html#valid-e-mail-address (HTML5 living standard, willful violation of RFC 3522)
        /// </summary>
        public static readonly string EmailValidation_Regex = @"^[a-zA-Z0-9.!#$%&'*+\/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$";

        public static readonly Regex EmailValidation_Regex_Compiled = new Regex(EmailValidation_Regex, RegexOptions.IgnoreCase);

        /// <summary>
        ///     Checks if the given e-mail is valid using various techniques https://github.com/Darkseal/EmailValidator/blob/main/EmailValidator.cs
        /// </summary>
        /// <param name="email">The e-mail address to check / validate</param>
        /// <param name="useRegEx">TRUE to use the HTML5 living standard e-mail validation RegEx, FALSE to use the built-in validator provided by .NET (default: FALSE)</param>
        /// <param name="requireDotInDomainName">TRUE to only validate e-mail addresses containing a dot in the domain name segment, FALSE to allow "dot-less" domains (default: FALSE)</param>
        /// <returns>TRUE if the e-mail address is valid, FALSE otherwise.</returns>
        public static bool IsValidEmailAddress(string email, bool useRegEx = true, bool requireDotInDomainName = true)
        {
            var isValid = useRegEx ? email is not null && EmailValidation_Regex_Compiled.IsMatch(email)
                // ref.: https://stackoverflow.com/a/33931538/1233379
                : new EmailAddressAttribute().IsValid(email);

            if (isValid && requireDotInDomainName)
            {
                var arr = email.Split('@', StringSplitOptions.RemoveEmptyEntries);
                isValid = arr.Length == 2 && arr[1].Contains(".");
            }
            return isValid;
        }

        public static async Task<List<Sheet>> validateAndCreateInviteForRows(
            DealerPortalContext Database,
            IMapper Mapper, Request request,
            List<InviteViewModel> output,
            List<Sheet> sheet,
            List<Sheet> failedOutput,
            List<string> dealerRoleTypes,
            List<string> accessLevelTypes,
            List<string> emailDealerNameDuplicate,
            List<Dealer> dealers,
            bool ignoreMsgFormulation = false)
        {
            List<Sheet> newSheets = new List<Sheet>();

            foreach (var row in sheet)
            {
                if (ignoreMsgFormulation == false)
                {
                    // Ensure all fields are filled
                    if (!row.GetType().GetProperties().Where(x => x.PropertyType == typeof(string)).All(p => p.GetValue(row) != null))
                    {
                        failedOutput = row.FormulateErrorMsg(failedOutput, "empty field(s)");
                        continue;
                    }

                    // Check if email is valid, though reason send failed will be added to invite if its invalid email
                    if (row.Email != null)
                        if (!IsValidEmailAddress(row.Email, request.TurnOffRegExForEmailValidation, request.TurnOffEmailRequireDotInDomainName))
                            failedOutput = row.FormulateErrorMsg(failedOutput, $"invalid email - {row.Email}");

                    // Check if email and dealer name combination is part of duplicate list
                    if (emailDealerNameDuplicate.Contains(row.Email + row.Dealership))
                        failedOutput = row.FormulateErrorMsg(failedOutput, $"duplicate email and dealer name combination - {row.Email} + {row.Dealership}");


                    // Check if role is valid
                    List<string> userDealerRoleTypes = row.UserRoles.Split(',').ToList();
                    var invalidDealerRoleTypes = userDealerRoleTypes.Where(x => !dealerRoleTypes.Contains(x)).ToList();

                    if (invalidDealerRoleTypes.Any())
                        failedOutput = row.FormulateErrorMsg(failedOutput, $"invalid dealer role type - {string.Join(", ", invalidDealerRoleTypes)}");

                    // Check if content access level types are valid 
                    List<string> userContentAccessLevels = row.UserAccessLevels.Split(',').ToList();
                    var invalidAccessLevels = userContentAccessLevels.Where(x => !accessLevelTypes.Contains(x)).ToList();

                    if (invalidAccessLevels.Any())
                        failedOutput = row.FormulateErrorMsg(failedOutput, $"invalid content access level - {string.Join(", ", invalidAccessLevels)}");

                    if (row.Dealership == "All")
                    {
                        var dealerCustAccs = dealers.Select(x => x.CustomerAccount).ToList();

                        foreach (var dlr in dealerCustAccs)
                        {
                            var newRow = Mapper.Map<Sheet>(row);
                            newRow.Dealership = dlr;
                            newRow.IgnoreExistingMembershipCheck = true;
                            newSheets.Add(newRow);
                        }

                        continue;
                    }

                }

                var dealer = row.Dealership == null ? null : dealers.FirstOrDefault(x => x.CustomerAccount == row.Dealership);

                if (dealer != null)
                {
                    Invite invite = null;

                    var user = await Database.Users.Include(x => x.Memberships).FirstOrDefaultAsync(u => u.Email == row.Email);

                    // User already exists, just add to dealer and mark the invite as claimed
                    if (user != null)
                    {
                        var membership = user.Memberships.FirstOrDefault(m => m.User.Email == user.Email && m.DealerId == dealer.Id);

                        invite = Mapper.Map<Invite>(row);

                        // Create membership, role and access levels for user if not exist yet
                        if (membership == null && user.EmailConfirmed)
                        {
                            // Create membership 
                            membership = new Membership { Id = Guid.NewGuid(), DealerId = dealer.Id, UserId = user.Id, User = user, ContextToken = Guid.Empty.ToString() };

                            membership.UpdateRolesAndAccessLevels(Database, row.UserRoles.Split(',').ToList(), row.UserAccessLevels.Split(',').ToList());
                            Database.Memberships.Add(membership);

                            MembershipNotification memberNotify;
                            memberNotify = new MembershipNotification();
                            memberNotify.Email = row.Email;
                            memberNotify.MembershipId = membership.Id;

                            Database.MembershipNotifications.Add(memberNotify);
                        }
                        else if (membership != null && user.EmailConfirmed == true)
                        {
                            failedOutput = row.IgnoreExistingMembershipCheck ? failedOutput : row.FormulateErrorMsg(failedOutput, $"existing membership for dealer - {dealer.CustomerAccount}");
                            continue;
                        }
                        else
                        {
                            // just adding comment to check whether new deployment will have any changes in env 
                            // this is to handel the senario user and membership is added in f&o  
                            invite = await Database.Invites.FirstOrDefaultAsync(i => i.Email == row.Email && i.Claimed == null);

                            if (invite == null)
                            {
                                invite = Mapper.Map<Invite>(row);
                                invite.DealerId = dealer.Id;
                                invite.CallbackUrl = string.Format(invite.CallbackUrl, invite.Token);
                                invite.UserRoles = string.Join(",", row.UserRoles);
                                invite.UserAccessLevels = string.Join(",", row.UserAccessLevels);
                                Database.Invites.Add(invite);
                            }
                            else
                            {
                                invite.Sent = null;
                                Database.Invites.Update(invite);
                            }

                            //addding roles
                            if (membership != null)
                                membership.UpdateRolesAndAccessLevels(Database, row.UserRoles.Split(',').ToList(), row.UserAccessLevels.Split(',').ToList());
                        }

                        output.Add(Mapper.Map<InviteViewModel>(invite));
                    }

                    // For new user added from dealer portal and without membership
                    else
                    {
                        invite = await Database.Invites.FirstOrDefaultAsync(i => i.Email == row.Email && i.Dealer.CustomerAccount == row.Dealership);

                        if (invite == null)
                        {
                            invite = Mapper.Map<Invite>(row);
                            invite.DealerId = dealer.Id;
                            invite.CallbackUrl = string.Format(request.CallbackUrl, invite.Token);
                            if (row.B2C == enableB2C)
                                invite.CallbackUrl += "?inviteB2C=true";
                            else
                                invite.CallbackUrl += "?inviteB2C=false";
                            invite.UserRoles = row.UserRoles;
                            invite.UserAccessLevels = row.UserAccessLevels;
                            Database.Invites.Add(invite);
                        }
                        else
                        {
                            // If it hasn't been claimed sent the email again
                            if (invite.Claimed == null)
                            {
                                Mapper.Map(row, invite);
                                invite.Sent = null;
                                invite.Attempts = 0;
                                invite.Failed = null;
                                invite.FailedMessage = null;
                                invite.LastAttempted = null;
                                Database.Invites.Update(invite);
                            }
                            else
                            {
                                failedOutput = row.FormulateErrorMsg(failedOutput, $"claimed invite for dealer name - {dealer.CustomerAccount}");
                                continue;
                            }
                        }
                        output.Add(Mapper.Map<InviteViewModel>(invite));
                    }
                }
                else
                {
                    failedOutput = row.FormulateErrorMsg(failedOutput, $"invalid dealership - {row.Dealership}");
                }
            }

            return newSheets;
        }

        public class RequestHandler : BaseRequestHandler<Request, Model>
        {
            public RequestHandler(DealerPortalContext dbContext, FunctionApiContext apiContext, IMapper mapper, IHttpContextAccessor httpContext, CurrentUserContext userAccessor) : base(dbContext, apiContext, mapper, httpContext, userAccessor)
            {
            }

            public override async Task<Model> Handle(Request request, CancellationToken cancellationToken)
            {
                #region Import Lines From Sheet

                List<Sheet> sheet = new List<Sheet>();
                try
                {
                    using (var stream = request.file.OpenReadStream())
                    {
                        using (var reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            // Read each row of the file
                            while (reader.Read())
                            {
                                // 1st row is always description
                                if (reader.Depth == 0 || reader.GetValue(0) == null)
                                {
                                    continue;
                                }

                                sheet.Add(new Sheet
                                {
                                    Index = reader.Depth,
                                    // According to dealer portal all fields cannot be null
                                    // Truncate emails with whitespace (take note if next row index is empty the concurrent row is ignored)
                                    Email = reader.GetValue(1) != null ? reader.GetValue(1).ToString().Trim() : null,
                                    Name = reader.GetValue(2) != null ? reader.GetValue(2).ToString().Trim() : null,
                                    B2C = reader.GetValue(3) != null ? reader.GetValue(3).ToString().Trim() : null,
                                    Dealership = reader.GetValue(4) != null ? reader.GetValue(4).ToString().Trim() : null,
                                    UserRoles = reader.GetValue(5) != null ? reader.GetValue(5).ToString().Replace(" ", "").Trim() : null,
                                    UserAccessLevels = reader.GetValue(6) != null ? reader.GetValue(6).ToString().Replace(" ", "").Trim() : null,
                                });
                            }
                        }
                    }
                }

                catch (Exception err)
                {
                    throw new RestException(HttpStatusCode.UnprocessableEntity, err.Message);
                }

                #endregion

                #region Validation And Creation

                // For performance reasons we have to limit the XLSX 
                if (sheet.Count > 100) throw new RestException(HttpStatusCode.UnprocessableEntity, $"XLSX file can't exceed 100 lines, current line count - {sheet.Count}");

                List<Sheet> failedOutput = new List<Sheet>();
                List<InviteViewModel> output = new List<InviteViewModel>();

                var dealerRoleTypes = await Database.DealerRoles.Select(x => x.Name).ToListAsync();
                var accessLevelTypes = await Database.AccessLevels.Select(x => x.Name).ToListAsync();
                var dealers = await Database.Dealers.ToListAsync();

                // Get duplicate email and dealer name combination list
                var emailDealerNameDuplicate = sheet.GroupBy(x => x.Email + x.Dealership).Where(g => g.Count() > 1).Select(y => y.Key).ToList();

                var newSheets = await validateAndCreateInviteForRows(Database, Mapper, request, output, sheet, failedOutput, dealerRoleTypes, accessLevelTypes, emailDealerNameDuplicate, dealers);

                if (newSheets.Any())
                {
                    await validateAndCreateInviteForRows(Database, Mapper, request, output, newSheets, failedOutput, dealerRoleTypes, accessLevelTypes, new List<string>(), dealers, true);
                    sheet.AddRange(newSheets);
                }

                // Transact pipeline and try cr8 membership in fno
                if (sheet.Count(x => x.HasError == true) == 0)
                {
                    await Database.SaveChangesAsync();

                    foreach (var row in sheet)
                    {
                        if (row.Dealership == "All")
                            continue;

                        var user = await Database.Users.Include(x => x.Memberships).ThenInclude(m => m.Dealer).FirstOrDefaultAsync(u => u.Email == row.Email);

                        if (user != null)
                        {
                            var membership = user.Memberships.FirstOrDefault(m => m.Dealer.CustomerAccount == row.Dealership);

                            var nameParts = membership.User.Name.Split(" ");
                            var firstName = nameParts[0];
                            var lastName = nameParts.Length > 1 ? nameParts[1] : "";

                            try
                            {
                                var result = await FunctionApi.Post("CreateMember", body: new
                                {
                                    FirstName = firstName,
                                    LastName = lastName,
                                    membership.User.Email,
                                    Dealer = membership.Dealer.CustomerAccount
                                });

                                membership.ContextToken = result.Headers.GetValues("User-Context-Token").First();
                            }
                            catch (FunctionApiException)
                            {
                                // This is expected if the member already exists ... not an ideal flow but works
                                membership.ContextToken = user.Memberships.First().ContextToken;
                            }
                        }
                    }
                }
                else
                    throw new RestException(HttpStatusCode.UnprocessableEntity, string.Join(" ", failedOutput.Select(x => x.ErrorMsg)));

                await Database.SaveChangesAsync();

                // Failed invites should be null!
                return new Model { Invites = output };

                #endregion
            }
        }
    }
}
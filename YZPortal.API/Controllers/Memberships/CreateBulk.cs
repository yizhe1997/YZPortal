using AutoMapper;
using ExcelDataReader;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net;
using YZPortal.Core.Domain.Database.Memberships;
using YZPortal.API.Infrastructure.Mediatr;
using YZPortal.Core.Error;
using YZPortal.Core.Domain.Contexts;

namespace YZPortal.Api.Controllers.Memberships
{
    public class CreateBulk
    {
        private static readonly string enableB2C = "Yes";

        public class Request : IRequest<Model>
        {
            [Required]
            internal IFormFile file { get; set; }
            // example: https://dealerportalfrontend.z13.web.core.windows.net/redeem/{0}
            [Required]
            public string CallbackUrl { get; set; }
        }

        public class Sheet
        {
            internal bool HasError { get; set; } = false;
            public int Index { get; set; }
            public string Email { get; set; }
            public string Name { get; set; }
            public string B2C { get; set; }
            public string DealerName { get; set; }
            public string UserRole { get; set; }
            public string UserContentAccessLevels { get; set; }
            internal string CallbackUrl { get; set; }
            // Added empty just for the no null field validation
            public string ErrorMsg { get; set; } = MembershipsHelper.emptyErrorMsg;
        }

        public class Model
        {
            public List<MembershipsCreateViewModel> Invites { get; set; }
            public List<Sheet> FailedInvites { get; set; }
        }

        public class RequestHandler : BaseRequestHandler<Request, Model>
        {
            public RequestHandler(PortalContext dbContext, IMapper mapper, IHttpContextAccessor httpContext, CurrentContext userAccessor) : base(dbContext, mapper, httpContext, userAccessor)
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
                                    Email = (reader.GetValue(1) != null) ? reader.GetValue(1).ToString().Trim() : null,
                                    Name = (reader.GetValue(2) != null) ? reader.GetValue(2).ToString().Trim() : null,
                                    B2C = (reader.GetValue(3) != null) ? reader.GetValue(3).ToString().Trim() : null,
                                    DealerName = (reader.GetValue(4) != null) ? reader.GetValue(4).ToString().Trim() : null,
                                    UserRole = (reader.GetValue(5) != null) ? reader.GetValue(5).ToString().Trim() : null,
                                    UserContentAccessLevels = (reader.GetValue(6) != null) ? reader.GetValue(6).ToString().Replace(" ", "").Trim() : null,
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
                if (sheet.Count > 100) throw new RestException(HttpStatusCode.UnprocessableEntity, $"XLSX file can't exceed 100 lines, current line count: {sheet.Count}");

                // To store rows that failed validation
                List<Sheet> failedOutput = new List<Sheet>();

                // To store each invites
                List<MembershipsCreateViewModel> output = new List<MembershipsCreateViewModel>();

                // Get valid access levels, roles, email from database
                var DealerRoleTypes = typeof(DealerRoleTypes).GetConstants();
                var ContentAccessLevelTypes = typeof(ContentAccessLevelTypes).GetConstants();
                var existingEmails = await Database.Users.Select(x => x.Email).ToListAsync();

                // Get duplicate email and dealer name combination list
                var emailDealerNameDuplicate = sheet.GroupBy(x => x.Email + x.DealerName).Where(g => g.Count() > 1).Select(y => y.Key).ToList();

                // Loop through each row in XLSX for validation and store the failed ones in a list
                foreach (var row in sheet)
                {
                    // Ensure all fields are filled
                    if (!row.GetType().GetProperties().Where(x => x.PropertyType == typeof(string)).All(p => p.GetValue(row) != null))
                        failedOutput = row.FormulateErrorMsg(failedOutput, "empty field(s)");

                    // Check if email is valid, though reason send failed will be added to invite if its invalid email
                    if (row.Email != null)
                        if (!CustomExtensions.IsValidEmailAddress(row.Email))
                            failedOutput = row.FormulateErrorMsg(failedOutput, $"invalid email : {row.Email}");

                    // Check if email and dealer name combination is part of duplicate list
                    if (!emailDealerNameDuplicate.Contains(row.Email + row.DealerName))
                        failedOutput = row.FormulateErrorMsg(failedOutput, $"duplicate email and dealer name combination : {row.Email} + {row.DealerName}");

                    // Check if role is valid
                    if (!DealerRoleTypes.Contains(row.UserRole))
                        failedOutput = row.FormulateErrorMsg(failedOutput, $"invalid role : {row.Email}");

                    // Check if content access level types are valid 
                    List<string> userContentAccessLevels = row.UserContentAccessLevels.Split(',').ToList();
                    var invalidContentAccessLevels = userContentAccessLevels.Where(x => !ContentAccessLevelTypes.Contains(x)).ToList();
                    
                    if (invalidContentAccessLevels.Any())
                        failedOutput = row.FormulateErrorMsg(failedOutput, $"invalid content access level : {string.Join(", ", invalidContentAccessLevels)}");

                    // Get all the exisitng dealers from the database and cr8 inv for all of the dealers
                    var dealer = row.DealerName == null ? null : await Database.Dealers.FirstAsync(x => x.Name == row.DealerName);

                    if (dealer != null)
                    {
                        MembershipInvite invite = null;

                        var user = await Database.Users.Include(x => x.Memberships).FirstOrDefaultAsync(u => u.Email == row.Email);

                        // User already exists, just add to dealer and mark the invite as claimed
                        if (user != null)
                        {
                            var membership = user.Memberships.FirstOrDefault(m => m.User.Email == user.Email && m.DealerId == dealer.Id);

                            // Create membership, role and access levels for user if not exist yet
                            if (membership == null && user.EmailConfirmed)
                            {
                                // Create membership 
                                membership = new Membership { Id = Guid.NewGuid(), DealerId = dealer.Id, UserId = user.Id, User = user };

                                membership.UpdateRolesAndContentAccessLevels(Database, row.UserRole, row.UserContentAccessLevels.Split(',').ToList());
                                Database.Memberships.Add(membership);

                                // Claim the invite                        
                                invite = Mapper.Map<MembershipInvite>(row);
                                invite.DealerId = dealer.Id;
                                invite.CallbackUrl = string.Format(request.CallbackUrl, invite.Token);
                                invite.UserRole = row.UserRole;
                                invite.UserContentAccessLevels = row.UserContentAccessLevels;
                                invite.MembershipId = membership.Id;
                                invite.ClaimedDateTime = DateTime.UtcNow;

                                Database.MembershipInvites.Add(invite);

                                // TO DO: RETARD ALERT
                                user.Name = invite.Name;
                                Database.Users.Update(user);
                            }

                            // Notify user to remove the user which already has membership and is confirmed
                            else if (membership != null && user.EmailConfirmed == true)
                            {
                                failedOutput = row.FormulateErrorMsg(failedOutput, $"existing membership for dealer name :{dealer.Name}");
                                continue;
                            }

                            output.Add(Mapper.Map<MembershipsCreateViewModel>(invite));
                        }

                        // For new user added from dealer portal and without membership
                        else
                        {
                            invite = await Database.MembershipInvites.FirstOrDefaultAsync(i => i.Email == row.Email && i.ClaimedDateTime == null);

                            if (invite == null)
                            {
                                invite = Mapper.Map<MembershipInvite>(row);
                                invite.DealerId = dealer.Id;
                                invite.CallbackUrl = string.Format(request.CallbackUrl, invite.Token);
                                if (row.B2C == enableB2C)
                                    invite.CallbackUrl += "?inviteB2C=true";
                                else
                                    invite.CallbackUrl += "?inviteB2C=false";
                                invite.UserRole = row.UserRole;
                                invite.UserContentAccessLevels = row.UserContentAccessLevels;
                                Database.MembershipInvites.Add(invite);
                            }
                            else
                            {
                                // If it hasn't been claimed sent the email again
                                if (invite.ClaimedDateTime == null)
                                {
                                    Mapper.Map(row, invite);
                                    invite.SentDateTime = null;
                                    invite.Attempts = 0;
                                    Database.MembershipInvites.Update(invite);
                                }
                                else
                                {
                                    failedOutput = row.FormulateErrorMsg(failedOutput, $"has claimed invite for dealer name :{dealer.Name}");
                                    continue;
                                }
                            }
                            output.Add(Mapper.Map<MembershipsCreateViewModel>(invite));
                        }

                    }
                }

                if (sheet.Count(x => x.HasError == true) == 0)
                {
                    await Database.SaveChangesAsync();
                }
                else
                    return new Model { Invites = null, FailedInvites = failedOutput };

                // Failed invites should ne null!
                return new Model { Invites = output, FailedInvites = failedOutput };

                #endregion
            }
        }
    }
}

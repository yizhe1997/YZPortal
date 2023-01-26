using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace YZPortal.API.Controllers.Memberships.Invites
{
    public class Create
    {
        public class Request : IRequest<Model>
        {
            public string Email { get; set; }
            public string Name { get; set; }
            public string CallbackUrl { get; set; } = "{0}";
            public List<string> UserRoles { get; set; } = new List<string> { };
            public List<string> UserAccessLevels { get; set; } = new List<string> { };
        }
        public class Model : InviteViewModel
        {
            public string WarningMessage { get; set; } = "";
        }
        public class RequestHandler : BaseRequestHandler<Request, Model>
        {
            public RequestHandler(DealerPortalContext dbContext, FunctionApiContext apiContext, IMapper mapper, IHttpContextAccessor httpContext, CurrentUserContext userAccessor) : base(dbContext, apiContext, mapper, httpContext, userAccessor)
            {
            }
            public override async Task<Model> Handle(Request request, CancellationToken cancellationToken)
            {
                Invite invite = null;
                // Truncate Emails with whitespace
                request.Email = request.Email.Trim();

                var user = await Database.Users.Include(x => x.Memberships).FirstOrDefaultAsync(u => u.Email == request.Email);

                // User already exists, just add to dealer and mark the invite as claimed
                var warningMessage = "";

                if (user != null)
                {
                    var membership = await UserContext.Memberships.IgnoreQueryFilters().FirstOrDefaultAsync(m => m.User.Email == request.Email);

                    invite = Mapper.Map<Invite>(request);

                    if (membership == null && user.EmailConfirmed)
                    {
                        // Create membership
                        membership = new Membership { DealerId = UserContext.CurrentDealerId, UserId = user.Id, User = user };
                        MembershipNotification memberNotify;

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
                                Dealer = UserContext.CurrentDealer.CustomerAccount
                            });

                            membership.ContextToken = result.Headers.GetValues("User-Context-Token").First();
                        }
                        catch (Exception err)
                        {
                            // This is expected if the member already exists ... not an ideal flow but works
                            if (err is FunctionApiException)
                            {
                                membership.ContextToken = user.Memberships.First().ContextToken;

                                var error = (FunctionApiException)err;
                                error.LogException(HttpContext.Request.Method + HttpContext.Request.Path, out string message);
                                warningMessage = message;
                            }
                            else
                            {
                                err.LogException(HttpContext.Request.Method + HttpContext.Request.Path, out string message);
                                warningMessage = message;
                            }
                        }

                        memberNotify = new MembershipNotification();
                        memberNotify.Email = request.Email;
                        memberNotify.MembershipId = membership.Id;

                        membership.UpdateRolesAndAccessLevels(Database, request.UserRoles, request.UserAccessLevels);
                        Database.Memberships.Add(membership);

                        await Database.SaveChangesAsync();

                        memberNotify.MembershipId = membership.Id;

                        Database.MembershipNotifications.Add(memberNotify);
                    }
                    else if (membership != null && user.EmailConfirmed == true)
                    {
                        throw new RestException(HttpStatusCode.UnprocessableEntity, "Membership already exists for user.");
                    }
                    else
                    {
                        // this is to handel the senario user and membership is added in f&o  
                        invite = await UserContext.Invites.FirstOrDefaultAsync(i => i.Email == request.Email && i.Claimed == null);

                        if (invite == null)
                        {
                            invite = Mapper.Map<Invite>(request);
                            invite.DealerId = UserContext.CurrentDealerId;
                            invite.CallbackUrl = string.Format(invite.CallbackUrl, invite.Token);
                            invite.UserRoles = string.Join(",", request.UserRoles);
                            invite.UserAccessLevels = string.Join(",", request.UserAccessLevels);
                            Database.Invites.Add(invite);
                        }
                        else
                        {
                            invite.Sent = null;
                            Database.Invites.Update(invite);
                        }

                        //addding roles
                        if (membership != null)
                            membership.UpdateRolesAndAccessLevels(Database, request.UserRoles, request.UserAccessLevels);
                    }

                    await Database.SaveChangesAsync();

                    return Mapper.Map<Model>(invite);
                }
                else
                {
                    // for new user added from dealer portal and without membership
                    invite = await UserContext.Invites.FirstOrDefaultAsync(i => i.Email == request.Email);

                    if (invite == null)
                    {
                        invite = Mapper.Map<Invite>(request);
                        invite.DealerId = UserContext.CurrentDealerId;
                        invite.CallbackUrl = string.Format(invite.CallbackUrl, invite.Token);
                        invite.UserRoles = string.Join(",", request.UserRoles);
                        invite.UserAccessLevels = string.Join(",", request.UserAccessLevels);
                        Database.Invites.Add(invite);
                    }
                    else
                    {
                        //If it hasn't been claimed sent the email again
                        if (invite.Claimed == null)
                        {
                            Mapper.Map(request, invite);
                            invite.Sent = null;
                            invite.Attempts = 0;
                            Database.Invites.Update(invite);
                        }
                        else
                        {
                            throw new RestException(HttpStatusCode.Conflict, ":This email has already claimed it's invite on - " + invite.Claimed);
                        }
                    }

                    await Database.SaveChangesAsync();

                    var output = Mapper.Map<Model>(invite);
                    output.WarningMessage += warningMessage;
                    return Mapper.Map<Model>(invite);
                }
            }
        }
    }
}

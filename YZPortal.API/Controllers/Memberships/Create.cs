using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;
using YZPortal.API.Infrastructure.Mediatr;
using YZPortal.Core.Domain.Contexts;
using YZPortal.Core.Domain.Database.Memberships;
using YZPortal.Core.Error;

namespace YZPortal.Api.Controllers.Memberships
{
    public class Create
    {
        public class Request : IRequest<Model>
        {
            public string Email { get; set; }
            public string Name { get; set; }
            public string CallbackUrl { get; set; } = "{0}";
            // One membership can only have one role
            public string UserRole { get; set; } = null;
            // One membership can have more than one content access levels
            public List<string> UserContentAccessLevels { get; set; } = new List<string> { };
        }

        public class Model : MembershipsCreateViewModel
        {
        }

        public class RequestHandler : BaseRequestHandler<Request, Model>
        {
            public RequestHandler(PortalContext dbContext, IMapper mapper, IHttpContextAccessor httpContext, CurrentContext userAccessor) : base(dbContext, mapper, httpContext, userAccessor)
            {
            }
            public override async Task<Model> Handle(Request request, CancellationToken cancellationToken)
            {
                MembershipInvite invite = null;

                // Truncate Emails with whitespace
                request.Email = request.Email.Trim();

                var user = await Database.Users.Include(x => x.Memberships).FirstOrDefaultAsync(u => u.Email == request.Email);

                // If user already exists, cr8 membership and mark the invite as claimed
                if (user != null)
                {
                    var membership = await CurrentContext.DealerMemberships.IgnoreQueryFilters().FirstOrDefaultAsync(m => m.User.Email == request.Email);

                    if (membership == null && user.EmailConfirmed)
                    {
                        // Create membership
                        membership = new Membership { DealerId = Core.Domain.Contexts.CurrentContext.DealerId, UserId = user.Id, User = user };

                        membership.UpdateRolesAndContentAccessLevels(Database, request.UserRole, request.UserContentAccessLevels);

                        Database.Memberships.Add(membership);

                        await Database.SaveChangesAsync();

                        // Claim the invite                        
                        invite = Mapper.Map<MembershipInvite>(request);
                        invite.Dealer.Id = Core.Domain.Contexts.CurrentContext.DealerId;
                        invite.CallbackUrl = string.Format(invite.CallbackUrl, invite.Token);
                        invite.UserRole = request.UserRole;
                        invite.UserContentAccessLevels = string.Join(",", request.UserContentAccessLevels);
                        invite.Membership.Id = membership.Id;
                        invite.ClaimedDateTime = DateTime.UtcNow;

                        Database.MembershipInvites.Add(invite);

                        user.Name = invite.Name;
                        Database.Users.Update(user);

                        await Database.SaveChangesAsync();
                    }
                    else if (membership != null && user.EmailConfirmed)
                    {
                        throw new RestException(HttpStatusCode.UnprocessableEntity, "Membership already exists for user.");
                    }

                    // if emailconfirmed is false
                    //else
                    //{
                    //    // just adding comment to check whether new deployment will have any changes in env 
                    //    // this is to handel the senario user and membership is added in f&o  
                    //    invite = await CurrentContext.DealerInvites.FirstOrDefaultAsync(i => i.Email == request.Email && i.ClaimedDateTime == null);

                    //    if (invite == null)
                    //    {
                    //        invite = Mapper.Map<MembershipInvite>(request);
                    //        invite.DealerId = CurrentContext.CurrentDealerId;
                    //        invite.CallbackUrl = string.Format(invite.CallbackUrl, invite.Token);
                    //        invite.UserRoles = string.Join(",", request.UserRoles);
                    //        invite.UserAccessLevels = string.Join(",", request.UserAccessLevels);
                    //        Database.Invites.Add(invite);
                    //    }
                    //    else
                    //    {
                    //        invite.SentDateTime = null;
                    //        Database.Invites.Update(invite);
                    //    }

                    //    //addding roles
                    //    if (membership != null)
                    //    {
                    //        if (request.UserRoles.Count == 0)
                    //        {
                    //            var role = Database.DealerRoles.FirstOrDefault(x => x.Name == "representative");

                    //            if (role != null)
                    //            {
                    //                Database.MembershipDealerRoles.Add(new MembershipDealerRole { MembershipId = membership.Id, DealerRoleId = role.Id });
                    //            }
                    //            if (request.UserAccessLevels.Count == 0)
                    //            {
                    //                var accessLevel = Database.AccessLevels.FirstOrDefault(x => x.Name == "All");
                    //                if (accessLevel != null)
                    //                {
                    //                    Database.MembershipAccessLevels.Add(new MembershipAccessLevel { MembershipId = membership.Id, AccessLevelId = accessLevel.Id });
                    //                }
                    //            }
                    //        }
                    //        else
                    //        {
                    //            foreach (string userRole in request.UserRoles)
                    //            {
                    //                var role = Database.DealerRoles.FirstOrDefault(x => x.Name == userRole);
                    //                if (role != null)
                    //                {
                    //                    Database.MembershipDealerRoles.Add(new MembershipDealerRole { MembershipId = membership.Id, DealerRoleId = role.Id });
                    //                }

                    //                foreach (var userAccess in request.UserAccessLevels)
                    //                {
                    //                    var accessLevel = Database.AccessLevels.FirstOrDefault(x => x.Name == userAccess);

                    //                    if (accessLevel != null)
                    //                    {
                    //                        Database.MembershipAccessLevels.Add(new MembershipAccessLevel { MembershipId = membership.Id, AccessLevelId = accessLevel.Id });
                    //                    }

                    //                }
                    //            }
                    //        }
                    //    }
                    //}

                    return Mapper.Map<Model>(invite);
                }
                else
                {
                    // for new user added from dealer portal and without membership 
                    // will overwrite existing invite for the new user if already invited
                    invite = await Core.Domain.Contexts.CurrentContext.DealerInvites.FirstOrDefaultAsync(i => i.Email == request.Email && i.ClaimedDateTime == null);

                    if (invite == null)
                    {
                        invite = Mapper.Map<MembershipInvite>(request);
                        invite.DealerId = Core.Domain.Contexts.CurrentContext.DealerId;
                        invite.CallbackUrl = string.Format(invite.CallbackUrl, invite.Token);
                        invite.UserRole = request.UserRole;
                        invite.UserContentAccessLevels = string.Join(",", request.UserContentAccessLevels);
                        Database.MembershipInvites.Add(invite);
                    }
                    else
                    {
                        // If it hasn't been claimed sent the email again
                        if (invite.ClaimedDateTime == null)
                        {
                            Mapper.Map(request, invite);
                            invite.SentDateTime = null;
                            invite.Attempts = 0;
                            Database.MembershipInvites.Update(invite);
                        }
                        else
                        {
                            throw new RestException(HttpStatusCode.Conflict, "This email has already claimed it's invite on: " + invite.ClaimedDateTime);
                        }
                    }

                    await Database.SaveChangesAsync();

                    return Mapper.Map<Model>(invite);
                }
            }
        }
    }
}

using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;
using YZPortal.API.Infrastructure.Mediatr;
using YZPortal.API.Infrastructure.Security.Jwt;
using YZPortal.Core.Domain.Contexts;
using YZPortal.Core.Domain.Database.Memberships;
using YZPortal.Core.Domain.Database.Users;
using YZPortal.Core.Error;

namespace YZPortal.Api.Controllers.Memberships
{
    public class Claim
    {
        public class Request : IRequest<Model>
        {
            public Guid Token { get; set; }
            public string Password { get; set; }
            public bool userLoginB2C { get; set; } = false;
        }

        public class Model
        {
            public string AuthToken { get; set; }
        }


        public class RequestHandler : BaseRequestHandler<Request, Model>
        {
            UserManager<User> UserManager { get; }
            JwtTokenGenerator JwtTokenGenerator { get; }

            public RequestHandler(PortalContext dbContext, IMapper mapper, IHttpContextAccessor httpContext, CurrentContext userAccessor, UserManager<User> userManager, JwtTokenGenerator jwtTokenGenerator) : base(dbContext, mapper, httpContext, userAccessor)
            {
                JwtTokenGenerator = jwtTokenGenerator;
                UserManager = userManager;
            }
            public override async Task<Model> Handle(Request request, CancellationToken cancellationToken)
            {
                #region User

                var invite = await Database.MembershipInvites
                    .Include(i => i.Dealer)
                    .FirstOrDefaultAsync(i => i.Token == request.Token);

                if (invite == null) throw new RestException(HttpStatusCode.NotFound);

                var user = await UserManager.FindByEmailAsync(invite.Email);

                // Create user with password if it does not exist
                if (user == null)
                {
                    user = new User { Email = invite.Email, Name = invite.Name, EmailConfirmed = true, Admin = false };

                    // Create the user
                    var createResult = (request.userLoginB2C == false) ? await UserManager.CreateAsync(user, request.Password)
                        : await UserManager.CreateAsync(user);

                    if (!createResult.Succeeded)
                    {
                        throw new RestException(HttpStatusCode.BadRequest, createResult.Errors.Select(e => e.Description.ToString()).ToList());
                    }
                }
                // In the scenario membership region fails we need to re-use the link? but based on our config
                // the process should revert if it already failed. Refer DbContextTransactionPipelineBehaviour
                else
                {
                    //// Mark the user as having the email confirmed
                    //user.EmailConfirmed = true;
                    //// below condition where user and membership added from the 
                    //if (user.PasswordHash == null && request.userLoginB2C == false)
                    //{
                    //    user.PasswordHash = UserManager.PasswordHasher.HashPassword(user, request.Password);
                    //    user.SecurityStamp = Guid.NewGuid().ToString();
                    //}
                    //await UserManager.UpdateAsync(user);

                    throw new RestException(HttpStatusCode.BadRequest, $"User with email {invite.Email} already exist!");
                }

                #endregion

                #region Membership

                var allInvites = await Database.MembershipInvites
                    .Include(i => i.Dealer)
                    .Where(i => i.Email == invite.Email)
                    .ToListAsync();

                var checkMembership = await Database.Memberships.IgnoreQueryFilters().FirstOrDefaultAsync(m => m.User.Email == user.Email);
                
                if (checkMembership == null)
                {
                    foreach (var inv in allInvites)
                    {
                        // Create membership
                        var membership = new Membership { Id = Guid.NewGuid(), Dealer.Id = inv.Dealer.Id, User.Id = user.Id, User = user };

                        string userRoles = inv.UserRole;
                        List<string> userAccessLevels = inv.UserContentAccessLevels.Split(',').ToList();

                        membership.UpdateRolesAndContentAccessLevels(Database, userRoles, userAccessLevels);

                        Database.Memberships.Add(membership);

                        await Database.SaveChangesAsync();

                        // Mark the invite as claimed
                        inv.ClaimedDateTime = DateTime.UtcNow;
                        inv.Membership.Id = membership.Id;
                        Database.MembershipInvites.Update(inv);
                    }

                }
                await Database.SaveChangesAsync();

                #endregion

                var jwtToken = await JwtTokenGenerator.CreateToken(user.Id.ToString());

                return new Model { AuthToken = jwtToken };
            }
        }
    }
}

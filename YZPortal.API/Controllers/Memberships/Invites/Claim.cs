using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace YZPortal.API.Controllers.Memberships.Invites
{
    public class Claim
    {
        public class Request : IRequest<Model>
        {
            public Guid Token { get; set; }
            public string Password { get; set; }
            // This will catter both azure ad and azure ad b2c as long its set to true
            public bool userLoginB2C { get; set; } = true;
        }
        public class Model
        {
            public string AuthToken { get; set; }
        }
        public class RequestHandler : BaseRequestHandler<Request, Model>
        {
            UserManager<User> UserManager { get; }
            JwtTokenGenerator JwtTokenGenerator { get; }

            public RequestHandler(DealerPortalContext dbContext, FunctionApiContext apiContext, IMapper mapper, IHttpContextAccessor httpContext, CurrentUserContext userAccessor, UserManager<User> userManager, JwtTokenGenerator jwtTokenGenerator) : base(dbContext, apiContext, mapper, httpContext, userAccessor)
            {
                JwtTokenGenerator = jwtTokenGenerator;
                UserManager = userManager;
            }
            public override async Task<Model> Handle(Request request, CancellationToken cancellationToken)
            {
                #region User

                var invite = await Database.Invites
                    .Include(i => i.Dealer)
                    .FirstOrDefaultAsync(i => i.Token == request.Token);

                if (invite == null) throw new RestException(HttpStatusCode.NotFound, "Invite not found.");

                var user = await UserManager.FindByEmailAsync(invite.Email);

                // Create user with password if it does not exist
                if (user == null)
                {
                    user = new User { Email = invite.Email, Name = invite.Name, EmailConfirmed = true, Admin = false };

                    // Create the user
                    var createResult = request.userLoginB2C == false ? await UserManager.CreateAsync(user, request.Password)
                        : await UserManager.CreateAsync(user);

                    if (!createResult.Succeeded)
                    {
                        throw new RestException(HttpStatusCode.BadRequest, createResult.Errors.Select(e => e.ToString()).ToList());
                    }
                }
                else
                {
                    // Mark the user as having the email confirmed
                    user.EmailConfirmed = true;
                    // below condition where user and membership added from the 
                    if (user.PasswordHash == null && request.userLoginB2C == false)
                    {
                        user.PasswordHash = UserManager.PasswordHasher.HashPassword(user, request.Password);
                        user.SecurityStamp = Guid.NewGuid().ToString();
                    }
                    await UserManager.UpdateAsync(user);
                }

                #endregion

                #region Membership

                var allInvites = await Database.Invites
                    .Include(i => i.Dealer)
                    .Where(i => i.Email == invite.Email)
                    .ToListAsync();


                var checkMembership = await Database.Memberships.IgnoreQueryFilters().FirstOrDefaultAsync(m => m.User.Email == user.Email);

                if (checkMembership == null)
                {
                    foreach (var inv in allInvites)
                    {
                        // Create membership
                        var membership = new Membership { DealerId = inv.DealerId, UserId = user.Id, User = user };

                        // Establish membership in FunctionApi
                        var nameParts = membership.User.Name.Split(" ");
                        var firstName = nameParts[0];
                        var lastName = nameParts.Length > 1 ? nameParts[1] : "";

                        var result = await FunctionApi.Post("CreateMember", body: new
                        {
                            FirstName = firstName,
                            LastName = lastName,
                            membership.User.Email,
                            Dealer = inv.Dealer.CustomerAccount
                        });

                        // string UserRoles = invite.UserRoles;
                        List<string> UserRoles = inv.UserRoles.Split(',').ToList();
                        List<string> UserAccessLevels = inv.UserAccessLevels.Split(',').ToList();

                        membership.ContextToken = result.Headers.GetValues("User-Context-Token").First();
                        user.ExternalId = result.Headers.GetValues("Member-Id").First();

                        membership.UpdateRolesAndAccessLevels(Database, UserRoles, UserAccessLevels);

                        Database.Memberships.Add(membership);

                        // Mark the invite as claimed
                        inv.Claimed = DateTime.UtcNow;
                    }

                    Database.Invites.UpdateRange(allInvites);
                }

                await Database.SaveChangesAsync();

                #endregion

                var jwtToken = await JwtTokenGenerator.CreateToken(user.Id.ToString());

                return new Model { AuthToken = jwtToken };
            }
        }
    }
}

using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;
using YZPortal.API.Controllers.Memberships;
using YZPortal.API.Infrastructure.Mediatr;
using YZPortal.Core.Domain.Contexts;
using YZPortal.Core.Domain.Database.Memberships;
using YZPortal.Core.Domain.Database.Users;
using YZPortal.Core.Error;
using YZPortal.Core.Extensions;
using YZPortal.FullStackCore.Enums.Memberships;

namespace YZPortal.API.Controllers.Dealers.DealerInvites
{
    public class Claim
    {
        public class Request : IRequest<Model>
        {
            public Guid Token { get; set; }
            public string? Password { get; set; }
            public bool IsExternalIdentity { get; set; }
            public int IdentityProvider { get; set; }
            public bool ClaimAllInvites { get; set; }
        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                var identityProviders = typeof(IdentityProviderNames).GetEnumDataTypeValues();
                RuleFor(x => x.IdentityProvider).NotNull().NotEmpty().GreaterThan(identityProviders.Min()).LessThanOrEqualTo(identityProviders.Max());
            }
        }

        public class Model
        {
        }

        public class RequestHandler : BaseRequestHandler<Request, Model>
        {
            UserManager<User> UserManager { get; }

            public RequestHandler(PortalContext dbContext, IMapper mapper, IHttpContextAccessor httpContext, CurrentContext userAccessor, UserManager<User> userManager) : base(dbContext, mapper, httpContext, userAccessor)
            {
                UserManager = userManager;
            }
            public override async Task<Model> Handle(Request request, CancellationToken cancellationToken)
            {
                #region User

                var invite = await Database.DealerInvites.FirstOrDefaultAsync(i => i.Token == request.Token);
                if (invite == null) throw new RestException(HttpStatusCode.NotFound, "Invite not found.");

                var user = await UserManager.FindByEmailAsync(invite.Email);
                // Create user with password if it does not exist
                if (user == null)
                {
                    user = new User { Email = invite.Email, Name = invite.Name, EmailConfirmed = true, Admin = false, IdentityProvider = request.IdentityProvider };

                    // Create the user
                    var createResult = request.IsExternalIdentity == false ? await UserManager.CreateAsync(user, request.Password)
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
                    if (user.PasswordHash == null && request.IsExternalIdentity == false)
                    {
                        user.PasswordHash = UserManager.PasswordHasher.HashPassword(user, request.Password);
                        user.SecurityStamp = Guid.NewGuid().ToString();
                    }
                    await UserManager.UpdateAsync(user);
                }

                #endregion

                #region Membership

                var invites = new List<DealerInvite>();
                if (request.ClaimAllInvites)
                {
                    invites = await Database.DealerInvites.Where(i => i.Email == invite.Email).ToListAsync();
                }
                else
                {
                    var inv = await CurrentContext.CurrentDealerInvites.FirstOrDefaultAsync(i => i.Email == invite.Email);
                    if (inv != null)
                        invites.Add(inv);
                }

                var checkMembership = await Database.Memberships.IgnoreQueryFilters().FirstOrDefaultAsync(m => m.User.Email == user.Email);
                if (checkMembership == null)
                {
                    foreach (var inv in invites)
                    {
                        // Create membership
                        var membership = new Membership { DealerId = inv.DealerId, UserId = user.Id };

                        // Update membership roles and content access levels
                        membership.UpdateRolesAndContentAccessLevels(Database, inv.UserRole, inv.UserContentAccessLevels.UnfoldBitmask<ContentAccessLevelNames>());

                        Database.Memberships.Add(membership);

                        // Mark the invite as claimed
                        inv.ClaimedDateTime = DateTime.UtcNow;
                    }
                }

                await Database.SaveChangesAsync();

                #endregion

                return new Model { };
            }
        }
    }
}

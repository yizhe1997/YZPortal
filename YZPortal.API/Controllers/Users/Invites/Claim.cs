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

namespace YZPortal.API.Controllers.Users.Invites
{
    public class Claim
    {
        public class Request : IRequest<Model>
        {
            public Guid Token { get; set; }
            public string? Name { get; set; } = "Anon";
            public string? Password { get; set; }
            public bool IsExternalIdentity { get; set; }
            public int IdentityProvider { get; set; }
        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                var identityProviders = typeof(IdentityProviderNames).GetEnumDataTypeValues();
                RuleFor(x => x.IdentityProvider).NotNull().GreaterThanOrEqualTo(identityProviders.Min()).LessThanOrEqualTo(identityProviders.Max());
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
                var invite = await Database.UserInvites.Include(x => x.UserInviteDealerSelections).FirstOrDefaultAsync(i => i.Token == request.Token);
                if (invite == null) throw new RestException(HttpStatusCode.NotFound, "Invite not found.");

                #region User

                var user = new User { Email = invite.Email, Name = request.Name, EmailConfirmed = true, Admin = false, IdentityProvider = request.IdentityProvider, UserInvite = invite };

                var createResult = request.IsExternalIdentity == false ? await UserManager.CreateAsync(user, request.Password)
                    : await UserManager.CreateAsync(user);

                if (!createResult.Succeeded)
                {
                    throw new RestException(HttpStatusCode.BadRequest, createResult.Errors.Select(e => e.ToString()).ToList());
                }


                #endregion

                #region Membership

                var dealerIdList = await Database.Dealers.Select(i => i.Id).ToListAsync();

                foreach (var userInviteDealerSelection in invite.UserInviteDealerSelections)
                {
                    if (dealerIdList.Contains(userInviteDealerSelection.DealerId))
                    {
                        // Create membership
                        var membership = new Membership { DealerId = userInviteDealerSelection.DealerId, UserId = user.Id, Id = Guid.NewGuid() };

                        // Update membership roles and content access levels
                        membership.UpdateRolesAndContentAccessLevels(Database, userInviteDealerSelection.UserRole, userInviteDealerSelection.UserContentAccessLevels.UnfoldBitmask<ContentAccessLevelNames>());

                        Database.Memberships.Add(membership);

                        // Mark the invite as claimed
                        invite.ClaimedDateTime = DateTime.UtcNow;
                    }
                }

                await Database.SaveChangesAsync();

                #endregion

                return new Model { };
            }
        }
    }
}

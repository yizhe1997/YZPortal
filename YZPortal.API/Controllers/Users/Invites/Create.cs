using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;
using YZPortal.API.Infrastructure.Mediatr;
using YZPortal.Core.Domain.Contexts;
using YZPortal.Core.Domain.Database.Users;
using YZPortal.Core.Error;
using YZPortal.FullStackCore.Enums.Memberships;
using static YZPortal.Core.Attributes.Attribute;

namespace YZPortal.API.Controllers.Users.Invites
{
    public class Create
    {
        public class Request : IRequest<Model>
        {
            public string? Email { get; set; }
            public string CallbackUrl { get; set; } = "{0}";
            public List<DealerSelection> DealerSelections { get; set; } = new List<DealerSelection>();
        }
        public class DealerSelection
        {
            public Guid DealerId { get; set; }
            public int Role { get; set; }
            [ListRequired]
            public List<int> ContentAccessLevels { get; set; } = new List<int>();
        }
        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.Email).NotNull().NotEmpty().EmailAddress();
            }
        }

        public class Model : InviteViewModel
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
                var user = await UserManager.FindByEmailAsync(request.Email);
                if (user != null)
                {
                    throw new RestException(HttpStatusCode.NotFound, $"User with email {request.Email} already exist!");
                }

                var checkInvite = await Database.UserInvites.FirstOrDefaultAsync(i => i.Email == request.Email);
                if (checkInvite != null)
                {
                    throw new RestException(HttpStatusCode.Conflict, $"Invitation for {request.Email} already exist!");
                }

                var invite = Mapper.Map<UserInvite>(request);
                invite.CallbackUrl = string.Format(invite.CallbackUrl, invite.Token);

                await AddDealerSelectionToInvite(invite, request.DealerSelections);

                Database.UserInvites.Add(invite);

                await Database.SaveChangesAsync();

                return Mapper.Map<Model>(invite);
            }

            private async Task AddDealerSelectionToInvite(UserInvite invite, List<DealerSelection> dealerSelections, bool removePrevDealerSelections = false)
            {
                if (dealerSelections.Any())
                {
                    if (removePrevDealerSelections)
                        invite.UserInviteDealerSelections.Clear();

                    var dealerIdList = await Database.Dealers.Select(i => i.Id).ToListAsync();
                    foreach (var dealerSelection in dealerSelections.OrderBy(x => x.DealerId))
                    {
                        if (dealerIdList.Contains(dealerSelection.DealerId))
                        {
                            invite.UserInviteDealerSelections.Add(new UserInviteDealerSelection
                            {
                                DealerId = dealerSelection.DealerId,
                                UserRole = dealerSelection.Role,
                                UserContentAccessLevels = dealerSelection.ContentAccessLevels.Aggregate(0, (current, n) => current | (int)(ContentAccessLevelNames)n)

                            });
                        }
                    }
                }
            }
        }
    }
}

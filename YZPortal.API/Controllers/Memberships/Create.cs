using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;
using YZPortal.Api.Controllers.Memberships;
using YZPortal.API.Infrastructure.Mediatr;
using YZPortal.Core.Domain.Contexts;
using YZPortal.Core.Domain.Database.Memberships;
using YZPortal.Core.Error;
using YZPortal.Core.Extensions;
using YZPortal.FullStackCore.Enums.Memberships;
using static YZPortal.Core.Attributes.Attribute;

namespace YZPortal.API.Controllers.Memberships
{
    public class Create
    {
        public class Request : IRequest<Model>
        {
            public Guid UserId { get; set; }
            public Guid DealerId { get; set; }
            public int Role { get; set; }
            [ListRequired]
            public List<int> ContentAccessLevels { get; set; } = new List<int>();
        }
        public class Validator : AbstractValidator<Request>
		{
			public Validator()
			{
				var dealerRoles = typeof(DealerRoleNames).GetEnumDataTypeValues();
				RuleFor(x => x.Role).NotEmpty().GreaterThan(dealerRoles.Min()).LessThanOrEqualTo(dealerRoles.Max());
			}
		}
		public class Model : MembershipViewModel
		{
        }
        public class RequestHandler : BaseRequestHandler<Request, Model>
        {
			public RequestHandler(PortalContext dbContext, IMapper mapper, IHttpContextAccessor httpContext, CurrentContext userAccessor) : base(dbContext, mapper, httpContext, userAccessor)
			{
			}
			public override async Task<Model> Handle(Request request, CancellationToken cancellationToken)
            {
                var user = await Database.Users.Include(x => x.Memberships).FirstOrDefaultAsync(u => u.Id == request.UserId);
                if (user == null)
				{
                    throw new RestException(HttpStatusCode.UnprocessableEntity, "User not found!");
                }

                var checkMembership = await Database.Memberships.Include(m => m.Dealer).FirstOrDefaultAsync(m => m.UserId == request.UserId && m.DealerId == request.DealerId);
                if (checkMembership != null)
                {
                    throw new RestException(HttpStatusCode.UnprocessableEntity, $"Membership for subscription {checkMembership.Dealer?.Name} already exist!");
                }

                // Create new membership and track
                var membership = new Membership { DealerId = CurrentContext.CurrentDealerId, UserId = user.Id, Id = Guid.NewGuid() };
                membership.UpdateRolesAndContentAccessLevels(Database, request.Role, request.ContentAccessLevels);
                Database.Memberships.Add(membership);

                // Create membership notification
                MembershipNotification membershipNotification = new MembershipNotification();
                membershipNotification.Email = user.Email;
                membershipNotification.MembershipId = membership.Id;
                Database.MembershipNotifications.Add(membershipNotification);

                await Database.SaveChangesAsync();

                return Mapper.Map<Model>(membership);
			}
        }
    }
}
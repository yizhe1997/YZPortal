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
using static YZPortal.Core.Attributes.Attribute;

namespace YZPortal.API.Controllers.Memberships
{
    public class Create
    {
        public class Request : IRequest<Model>
        {
            public string? Name { get; set; }
            public string? CallbackUrl { get; set; } = "{0}";
            public int Role { get; set; }
			[ListRequired]
			public List<int> ContentAccessLevels { get; set; } = new List<int>();
        }
		public class Validator : AbstractValidator<Request>
		{
			public Validator()
			{
				RuleFor(x => x.Name).NotNull().NotEmpty();
				RuleFor(x => x.Role).NotNull().NotEmpty();
				var dealerRoles = typeof(DealerRoleNames).GetEnumDataTypeValues();
				RuleFor(x => x.Role).NotEmpty().GreaterThan(dealerRoles.Min()).LessThanOrEqualTo(dealerRoles.Max());
			}
		}
		public class Model : MembershipsViewModel
		{
        }
        public class RequestHandler : BaseRequestHandler<Request, Model>
        {
			public RequestHandler(PortalContext dbContext, IMapper mapper, IHttpContextAccessor httpContext, CurrentContext userAccessor) : base(dbContext, mapper, httpContext, userAccessor)
			{
			}
			public override async Task<Model> Handle(Request request, CancellationToken cancellationToken)
            {
				MembershipInvite? invite = null;

				var user = await Database.Users.Include(x => x.Memberships).FirstOrDefaultAsync(u => u.Email == CurrentContext.CurrentUser.Email);

				// User already exists, just add to dealer and mark the invite as claimed
				if (user != null)
				{
					var membership = await CurrentContext.CurrentDealerMemberships.IgnoreQueryFilters().FirstOrDefaultAsync(m => m.User.Email == CurrentContext.CurrentUser.Email);

					invite = Mapper.Map<MembershipInvite>(request);

					if (membership == null && user.EmailConfirmed)
					{
						// Create new membership and track
						membership = new Membership { DealerId = CurrentContext.CurrentDealerId, UserId = user.Id, Id = new Guid() };
						membership.UpdateRolesAndContentAccessLevels(Database, request.Role, request.ContentAccessLevels);
						Database.Memberships.Add(membership);

						// Create membership notification
						MembershipNotification membershipNotification = new MembershipNotification();
						membershipNotification.Email = CurrentContext.CurrentUser.Email;
						membershipNotification.MembershipId = membership.Id;
						Database.MembershipNotifications.Add(membershipNotification);

						await Database.SaveChangesAsync();
					}
					else if (membership != null && user.EmailConfirmed == true)
					{
						throw new RestException(HttpStatusCode.UnprocessableEntity, "Membership already exist for user.");
					}
				}
				else
				{
					// For new user added from dealer portal and without membership
					invite = await CurrentContext.CurrentDealerInvites.FirstOrDefaultAsync(i => i.Email == CurrentContext.CurrentUser.Email);

					if (invite == null)
					{
						invite = Mapper.Map<MembershipInvite>(request);
						invite.DealerId = CurrentContext.CurrentDealerId;
						invite.CallbackUrl = string.Format(invite.CallbackUrl, invite.Token);
						invite.UserRole = request.Role;
						invite.UserContentAccessLevels = request.ContentAccessLevels.Aggregate(0, (current, n) => current | (int)(ContentAccessLevelNames)n);
						Database.MembershipInvites.Add(invite);
					}
					// If it hasn't been claimed sent the email again
					else
					{
						if (invite.ClaimedDateTime == null)
						{
							Mapper.Map(request, invite);
							invite.SentDateTime = null;
							invite.Attempts = 0;
						}
						else
						{
							throw new RestException(HttpStatusCode.Conflict, "Email invitation already claimed on - " + invite.ClaimedDateTime);
						}
					}

					await Database.SaveChangesAsync();
				}
				return Mapper.Map<Model>(invite);
			}
        }
    }
}

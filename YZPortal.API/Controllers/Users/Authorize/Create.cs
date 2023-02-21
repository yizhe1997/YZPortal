using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using YZPortal.API.Infrastructure.Mediatr;
using YZPortal.API.Infrastructure.Security.Jwt;
using YZPortal.Core.Domain.Contexts;

namespace YZPortal.API.Controllers.Users.Authorize
{
    public class Create
    {
        public class Request : IRequest<Model>
        {
            public Guid DealerId { get; set; }
        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.DealerId).NotNull().NotEmpty();
            }
        }

        public class Model : AuthorizeViewModel
		{
        }

        public class RequestHandler : BaseRequestHandler<Request, Model>
        {
            JwtTokenGenerator JwtTokenGenerator { get; }

            public RequestHandler(PortalContext dbContext, IMapper mapper, IHttpContextAccessor httpContext, CurrentContext userAccessor, JwtTokenGenerator jwtTokenGenerator) : base(dbContext, mapper, httpContext, userAccessor)
            {
                JwtTokenGenerator = jwtTokenGenerator;
            }

            public override async Task<Model> Handle(Request request, CancellationToken cancellationToken)
            {
                var membership = await CurrentContext.CurrentUserMemberships
                    .Include(x => x.MembershipDealerRole)
                    .ThenInclude(x => x.DealerRole)
                    .Include(x => x.MembershipContentAccessLevels)
                    .ThenInclude(m => m.ContentAccessLevel)
                    .Where(x => x.DealerId == request.DealerId)
                    .FirstOrDefaultAsync();

                var claims = await membership.CreateClaimsForMembership(Database, CurrentContext);

                var token = await JwtTokenGenerator.CreateToken(membership?.UserId.ToString() ?? string.Empty, claims);

                return new Model { AuthToken = token };
            }
        }
    }
}

using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using YZPortal.API.Infrastructure.Mediatr;
using YZPortal.Core.Domain.Contexts;

namespace YZPortal.Api.Controllers.Memberships
{
    public class Details
    {
        public class Request : IRequest<Model>
        {
            internal Guid Id { get; set; }
        }

        public class Model : MembershipViewModel
        {
        }

        public class RequestHandler : BaseRequestHandler<Request, Model>
        {
            public RequestHandler(PortalContext dbContext, IMapper mapper, IHttpContextAccessor httpContext, CurrentContext userAccessor) : base(dbContext, mapper, httpContext, userAccessor)
            {
            }

            public async override Task<Model> Handle(Request request, CancellationToken cancellationToken)
            {
                var membership = await CurrentContext.CurrentDealerMemberships
                    .Include(x => x.MembershipDealerRole)
                    .ThenInclude(x => x.DealerRole)
                    .Include(m => m.MembershipContentAccessLevels)
                    .ThenInclude(m => m.ContentAccessLevel)
                    .FirstOrDefaultAsync(x => x.Id == request.Id);

                return Mapper.Map<Model>(membership);
            }
        }
    }
}

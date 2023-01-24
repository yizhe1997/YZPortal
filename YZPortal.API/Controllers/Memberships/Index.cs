using AutoMapper;
using Microsoft.EntityFrameworkCore;
using YZPortal.Core.Domain.Contexts;
using YZPortal.Core.Domain.Database.Memberships;

namespace YZPortal.Api.Controllers.Memberships
{
    public class Index
    {
        public class Request : SearchRequest<SearchResponse<Model>>
        {
        }
        public class Model : MembershipsViewModel
        {
        }
        public class RequestHandler : SearchRequestHandler<Request, SearchResponse<Model>>
        {
            public RequestHandler(PortalContext dbContext, IMapper mapper, IHttpContextAccessor httpContext, CurrentContext userAccessor) : base(dbContext, mapper, httpContext, userAccessor)
            {
            }
            public override async Task<SearchResponse<Model>> Handle(Request request, CancellationToken cancellationToken)
            {
                return await CreateIndexResponseAsync<Membership, Model>(
                     request,
                     CurrentContext.DealerMemberships
                         .Include(x => x.MembershipDealerRole)
                         .ThenInclude(x => x.DealerRole)
                         .Include(m => m.MembershipContentAccessLevels)
                         .ThenInclude(m => m.ContentAccessLevel)
                         .Include(x => x.User)
                         .Include(x => x.Dealer)
                         .Where(x => x.User.Admin != true),
                     x => x.User.Email.Contains(request.SearchString) ||
                          x.User.Name.Contains(request.SearchString));
            }
        }
    }
}

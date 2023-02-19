using AutoMapper;
using YZPortal.API.Infrastructure.Mediatr;
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
                var query = CurrentContext.CurrentDealerMemberships;

                return await CreateIndexResponseAsync<Membership, Model>(
                     request,
                     query);
            }
        }
    }
}

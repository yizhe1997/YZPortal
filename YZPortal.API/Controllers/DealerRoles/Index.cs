using AutoMapper;
using YZPortal.Core.Domain.Contexts;
using YZPortal.Core.Domain.Database.Memberships;

namespace YZPortal.Api.Controllers.DealerRoles
{
    public class Index
    {
        public class Request : SearchRequest<SearchResponse<Model>>
        {
        }

        public class Model : DealerRolesViewModel
        {
        }

        public class RequestHandler : SearchRequestHandler<Request, SearchResponse<Model>>
        {
            public RequestHandler(PortalContext dbContext, IMapper mapper, IHttpContextAccessor httpContext, CurrentContext userAccessor) : base(dbContext, mapper, httpContext, userAccessor)
            {
            }

            public override async Task<SearchResponse<Model>> Handle(Request request, CancellationToken cancellationToken)
            {
                return await CreateIndexResponseAsync<DealerRole, Model>(
                    request,
                    Database.DealerRoles,
                    x => x.Name == request.SearchString);
            }
        }
    }
}

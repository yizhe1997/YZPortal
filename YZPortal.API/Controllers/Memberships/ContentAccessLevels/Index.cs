using AutoMapper;
using YZPortal.API.Controllers.Pagination;
using YZPortal.API.Infrastructure.Mediatr;
using YZPortal.Core.Domain.Contexts;
using YZPortal.Core.Domain.Database.Memberships;
using YZPortal.FullStackCore.Enums.Memberships;

namespace YZPortal.API.Controllers.Memberships.ContentAccessLevels
{
    public class Index
    {
        public class Request : SearchRequest<SearchResponse<Model>>
        {
        }

        public class Model : ContentAccessLevelViewModel
        {
        }

        public class RequestHandler : SearchRequestHandler<Request, SearchResponse<Model>>
        {
            public RequestHandler(PortalContext dbContext, IMapper mapper, IHttpContextAccessor httpContext, CurrentContext userAccessor) : base(dbContext, mapper, httpContext, userAccessor)
            {
            }

            public override async Task<SearchResponse<Model>> Handle(Request request, CancellationToken cancellationToken)
            {
                return await CreateIndexResponseAsync<ContentAccessLevel, Model>(
                    request,
                    Database.ContentAccessLevels,
                    x => ((ContentAccessLevelNames)x.Name).ToString() == request.SearchString);
            }
        }
    }
}

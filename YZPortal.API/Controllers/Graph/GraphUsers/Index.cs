using AutoMapper;
using YZPortal.API.Controllers.Pagination;
using YZPortal.API.Infrastructure.Mediatr;
using YZPortal.Core.Domain.Contexts;
using YZPortal.Core.Graph;
using YZPortal.FullStackCore.Models.Abstracts;
using YZPortal.FullStackCore.Models.Graph.Users;

namespace YZPortal.API.Controllers.Graph.GraphUsers
{
    public class Index
    {
        public class Request : SearchRequest<SearchModel<Model>>
        {
        }
        public class Model : GraphUserModel
        {
        }
        public class RequestHandler : SearchRequestHandler<Request, SearchModel<Model>>
        {
            private readonly GraphClientProvider _graphClientProvider;

            public RequestHandler(PortalContext dbContext, IMapper mapper, IHttpContextAccessor httpContext, CurrentContext userAccessor, GraphClientProvider graphClientProvider) : base(dbContext, mapper, httpContext, userAccessor)
            {
                _graphClientProvider = graphClientProvider;

            }
            public override async Task<SearchModel<Model>> Handle(Request request, CancellationToken cancellationToken)
            {
                var users = await _graphClientProvider.GetUsers();
                var usersMapped = Mapper.Map<List<Model>>(users);

                return CreateIndexResponse(request, usersMapped);
            }
        }
    }
}

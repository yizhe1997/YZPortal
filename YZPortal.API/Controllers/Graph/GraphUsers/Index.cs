using AutoMapper;
using YZPortal.API.Controllers.ControllerRequests.Indexes;
using YZPortal.API.Infrastructure.Mediatr;
using YZPortal.Core.Domain.Contexts;
using YZPortal.Core.Domain.Database;
using YZPortal.Core.Graph;
using YZPortal.FullStackCore.Models.Abstracts;
using YZPortal.FullStackCore.Models.Graph.Users;

namespace YZPortal.API.Controllers.Graph.GraphUsers
{
    public class Index
    {
        public class Request : SearchRequest<SearchModel<GraphUserModel>>
        {
        }
        public class RequestHandler : SearchRequestHandler<Request, SearchModel<GraphUserModel>>
        {
            private readonly GraphClientProvider _graphClientProvider;

            public RequestHandler(DatabaseService dbService, IMapper mapper, IHttpContextAccessor httpContext, CurrentContext userAccessor, GraphClientProvider graphClientProvider) : base(dbService, mapper, httpContext, userAccessor)
            {
                _graphClientProvider = graphClientProvider;
            }
            public override async Task<SearchModel<GraphUserModel>> Handle(Request request, CancellationToken cancellationToken)
            {
                // Get users
                var users = await _graphClientProvider.UsersToSearchListAsync(
                    request,
                    x => x.DisplayName.Contains(request.SearchString)
                    , cancellationToken);

                // Return mapped model
                return Mapper.Map<SearchModel<GraphUserModel>>(users);
            }
        }
    }
}

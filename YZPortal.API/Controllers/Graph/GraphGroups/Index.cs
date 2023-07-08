using AutoMapper;
using YZPortal.API.Controllers.ControllerRequests.Indexes;
using YZPortal.API.Infrastructure.Mediatr;
using YZPortal.Core.Domain.Contexts;
using YZPortal.Core.Domain.Database;
using YZPortal.Core.Graph;
using YZPortal.FullStackCore.Models.Abstracts;
using YZPortal.FullStackCore.Models.Graph.Groups;

namespace YZPortal.API.Controllers.Graph.GraphGroups
{
    public class Index
	{
		public class Request : SearchRequest<SearchModel<GraphGroupModel>>
		{
			public string? GraphUserId { get; set; }
		}
		public class RequestHandler : SearchRequestHandler<Request, SearchModel<GraphGroupModel>>
		{
            private readonly GraphClientProvider _graphClientProvider;

            public RequestHandler(DatabaseService dbService, IMapper mapper, IHttpContextAccessor httpContext, CurrentContext userAccessor, GraphClientProvider graphClientProvider) : base(dbService, mapper, httpContext, userAccessor)
			{
                _graphClientProvider = graphClientProvider;
            }
			public override async Task<SearchModel<GraphGroupModel>> Handle(Request request, CancellationToken cancellationToken)
			{
                // If request.GraphUserId is not null or empty, then get groups for that user. Else get all groups
                var groups = (!string.IsNullOrEmpty(request.GraphUserId)) ? 
					await _graphClientProvider.UserGroupsToSearchListAsync(
                        request.GraphUserId,
                        request,
                        x => x.DisplayName.Contains(request.SearchString)
                        , cancellationToken) : 
					await _graphClientProvider.GroupsToSearchListAsync(
                        request,
                        x => x.DisplayName.Contains(request.SearchString)
                        , cancellationToken);

                // Return mapped model
                return Mapper.Map<SearchModel<GraphGroupModel>>(groups);
            }
		}
	}
}

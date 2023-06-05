using AutoMapper;
using YZPortal.API.Controllers.Pagination;
using YZPortal.API.Infrastructure.Mediatr;
using YZPortal.Core.Domain.Contexts;
using YZPortal.Core.Graph;

namespace YZPortal.API.Controllers.Graph.GraphGroups
{
    public class Index
	{
		public class Request : SearchRequest<SearchResponse<Model>>
		{
			public string? GraphUserId { get; set; }
		}
		public class Model : GraphGroupViewModel
		{
		}
		public class RequestHandler : SearchRequestHandler<Request, SearchResponse<Model>>
		{
            private readonly GraphClientProvider _graphClientProvider;

            public RequestHandler(PortalContext dbContext, IMapper mapper, IHttpContextAccessor httpContext, CurrentContext userAccessor, GraphClientProvider graphClientProvider) : base(dbContext, mapper, httpContext, userAccessor)
			{
                _graphClientProvider = graphClientProvider;

            }
			public override async Task<SearchResponse<Model>> Handle(Request request, CancellationToken cancellationToken)
			{
				// Vars
                var groupsMapped = new List<Model>();

                // If request.GraphUserId is not null or empty, then get groups for that user. Else get all groups
                var groups = (!string.IsNullOrEmpty(request.GraphUserId)) ? await _graphClientProvider.GetUserGroups(request.GraphUserId) : await _graphClientProvider.GetGroups();

                // Map groups from Graph
                Mapper.Map(groups, groupsMapped);

                // Return index reponse
                return CreateIndexResponse(request, groupsMapped);
            }
		}
	}
}

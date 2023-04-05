using AutoMapper;
using YZPortal.API.Controllers.Pagination;
using YZPortal.API.Infrastructure.Mediatr;
using YZPortal.Core.Domain.Contexts;
using YZPortal.Core.Domain.Database.Users;

namespace YZPortal.API.Controllers.Users
{
	public class Index
	{
		public class Request : SearchRequest<SearchResponse<Model>>
		{
		}
		public class Model : UserViewModel
		{
		}
		public class RequestHandler : SearchRequestHandler<Request, SearchResponse<Model>>
		{
			public RequestHandler(PortalContext dbContext, IMapper mapper, IHttpContextAccessor httpContext, CurrentContext userAccessor) : base(dbContext, mapper, httpContext, userAccessor)
			{
			}
			public override async Task<SearchResponse<Model>> Handle(Request request, CancellationToken cancellationToken)
			{
				// Only get non-admin users to prevent self-deletion from client app
				var query = Database.Users.Where(x => x.Admin == false).AsQueryable();

				return await CreateIndexResponseAsync<User, Model>(
					request,
					query,
					x => x.Name.Contains(request.SearchString) ||
					x.UserName.Contains(request.SearchString));
			}
		}
	}
}

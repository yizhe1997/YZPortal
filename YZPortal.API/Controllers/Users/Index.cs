using AutoMapper;
using YZPortal.API.Controllers.Pagination;
using YZPortal.API.Infrastructure.Mediatr;
using YZPortal.Core.Domain.Contexts;
using YZPortal.Core.Domain.Database.Users;
using YZPortal.FullStackCore.Models.Abstracts;
using YZPortal.FullStackCore.Models.Users;

namespace YZPortal.API.Controllers.Users
{
    public class Index
	{
		public class Request : SearchRequest<SearchModel<Model>>
		{
		}
		public class Model : UserModel
		{
		}
		public class RequestHandler : SearchRequestHandler<Request, SearchModel<Model>>
		{
			public RequestHandler(PortalContext dbContext, IMapper mapper, IHttpContextAccessor httpContext, CurrentContext userAccessor) : base(dbContext, mapper, httpContext, userAccessor)
			{
			}
			public override async Task<SearchModel<Model>> Handle(Request request, CancellationToken cancellationToken)
			{
				var query = Database.Users.AsQueryable();

				return await CreateIndexResponseAsync<User, Model>(
					request,
					query,
					x => x.DisplayName.Contains(request.SearchString) ||
					x.UserName.Contains(request.SearchString));
			}
		}
	}
}

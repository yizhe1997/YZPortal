using AutoMapper;
using YZPortal.API.Controllers.ControllerRequests.Indexes;
using YZPortal.API.Infrastructure.Mediatr;
using YZPortal.Core.Domain.Contexts;
using YZPortal.Core.Domain.Database;
using YZPortal.FullStackCore.Models.Abstracts;
using YZPortal.FullStackCore.Models.Users;

namespace YZPortal.API.Controllers.Users
{
    public class Index
	{
		public class Request : SearchRequest<SearchModel<UserModel>>
		{
		}
		public class RequestHandler : SearchRequestHandler<Request, SearchModel<UserModel>>
		{
            public RequestHandler(DatabaseService dbService, IMapper mapper, IHttpContextAccessor httpContext, CurrentContext userAccessor) : base(dbService, mapper, httpContext, userAccessor)
            {
            }
            public override async Task<SearchModel<UserModel>> Handle(Request request, CancellationToken cancellationToken)
			{
                // Get users
                var users = await DatabaseService.UsersToSearchListAsync(
                    request,
                    x => x.DisplayName.Contains(request.SearchString) ||
                    x.UserName.Contains(request.SearchString)
                    , cancellationToken);

                // Return mapped model
                return Mapper.Map<SearchModel<UserModel>>(users);
            }
		}
	}
}

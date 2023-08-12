using AutoMapper;
using MediatR;
using YZPortal.API.Infrastructure.Mediatr;
using YZPortal.Core.Domain.Contexts;
using YZPortal.Core.Domain.Database;
using YZPortal.Core.Domain.Database.Users;
using YZPortal.Core.Graph;
using YZPortal.FullStackCore.Models.Users;
using YZPortal.FullStackCore.Requests.Users;

namespace YZPortal.API.Controllers.Users
{
    public class Update
	{
		public class Request : UpdateUserRequest, IRequest<UserModel>
        {
		}
		public class RequestHandler : BaseRequestHandler<Request, UserModel>
		{
            private readonly GraphClientProvider _graphClientProvider;
            public RequestHandler(DatabaseService dbService, IMapper mapper, IHttpContextAccessor httpContext, CurrentContext userAccessor, GraphClientProvider graphClientProvider) : base(dbService, mapper, httpContext, userAccessor)
            {
                _graphClientProvider = graphClientProvider;
            }
            public override async Task<UserModel> Handle(Request request, CancellationToken cancellationToken)
			{
                User user;

                if (request.IsAuthenEvent)
                {
                    // Update user via b2c token
                    user = await DatabaseService.UserUpdateAsync(request.SubjectId, CurrentContext, cancellationToken);
                }
                else
                {
                    // Update user via http request
                    user = await DatabaseService.UserUpdateAsync(request.SubjectId, request, cancellationToken);

                    // Update user from graph
                    await _graphClientProvider.UserUpdateAsync(request.SubjectId, request, cancellationToken);
                }

                // Return mapped model
                return Mapper.Map<UserModel>(user);
			}
		}
	}
}

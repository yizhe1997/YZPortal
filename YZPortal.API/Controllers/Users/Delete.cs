using AutoMapper;
using MediatR;
using YZPortal.API.Infrastructure.Mediatr;
using YZPortal.Core.Domain.Contexts;
using YZPortal.Core.Domain.Database;
using YZPortal.Core.Graph;
using YZPortal.FullStackCore.Models.Users;

namespace YZPortal.API.Controllers.Users
{
    public class Delete
	{
		public class Request : IRequest<UserModel>
		{
			internal Guid Id { get; set; }
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
				// Delete user from db
				var user = await DatabaseService.UserDeleteAsync(request.Id, cancellationToken);
                
				// Delete user from graph
				await _graphClientProvider.UserDeleteAsync(user.SubjectIdentifier, cancellationToken);

				// Return mapped model
                return Mapper.Map<UserModel>(user);
			}
		}
	}
}

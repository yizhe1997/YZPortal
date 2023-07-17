using AutoMapper;
using MediatR;
using YZPortal.API.Infrastructure.Mediatr;
using YZPortal.Core.Domain.Contexts;
using YZPortal.Core.Domain.Database;
using YZPortal.FullStackCore.Models.Users;

namespace YZPortal.API.Controllers.Users
{
    public class Update
	{
		public class Request : IRequest<UserModel>
		{
			internal string? SubjectId { get; set; }
		}
		public class RequestHandler : BaseRequestHandler<Request, UserModel>
		{
            public RequestHandler(DatabaseService dbService, IMapper mapper, IHttpContextAccessor httpContext, CurrentContext userAccessor) : base(dbService, mapper, httpContext, userAccessor)
            {
            }
            public override async Task<UserModel> Handle(Request request, CancellationToken cancellationToken)
			{
                // Update user via b2c token
                var user = await DatabaseService.UserUpdateAsync(request.SubjectId, CurrentContext, cancellationToken);

                // Return mapped model
                return Mapper.Map<UserModel>(user);
			}
		}
	}
}

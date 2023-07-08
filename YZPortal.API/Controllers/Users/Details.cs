using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using YZPortal.API.Infrastructure.Mediatr;
using YZPortal.Core.Domain.Contexts;
using YZPortal.Core.Domain.Database;
using YZPortal.Core.Domain.Database.Users;
using YZPortal.FullStackCore.Models.Users;

namespace YZPortal.API.Controllers.Users
{
    public class Details
	{
		public class Request : IRequest<UserModel>
		{
			internal Guid SubjectId { get; set; }
		}
		public class RequestHandler : BaseRequestHandler<Request, UserModel>
		{
            public RequestHandler(DatabaseService dbService, IMapper mapper, IHttpContextAccessor httpContext, CurrentContext userAccessor) : base(dbService, mapper, httpContext, userAccessor)
            {
            }
            public override async Task<UserModel> Handle(Request request, CancellationToken cancellationToken)
			{
				// Get user from db
                var user = await DatabaseService.UserGetAsync(request.SubjectId, cancellationToken);

                // Return mapped model
                return Mapper.Map<UserModel>(user);
			}
		}
	}
}

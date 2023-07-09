using AutoMapper;
using MediatR;
using YZPortal.API.Infrastructure.Mediatr;
using YZPortal.Core.Domain.Contexts;
using YZPortal.Core.Domain.Database;
using YZPortal.FullStackCore.Models.Users;

namespace YZPortal.API.Controllers.Users
{
    public class Create
    {
        public class Request : IRequest<UserModel>
        {
        }
        public class RequestHandler : BaseRequestHandler<Request, UserModel>
        {
            public RequestHandler(DatabaseService dbService, IMapper mapper, IHttpContextAccessor httpContext, CurrentContext userAccessor) : base(dbService, mapper, httpContext, userAccessor)
            {
            }
            public override async Task<UserModel> Handle(Request request, CancellationToken cancellationToken)
            {
                // Create user via b2c token
                var user = await DatabaseService.UserCreateAsync(CurrentContext, cancellationToken);

                // Return mapped model
                return Mapper.Map<UserModel>(user);
            }
        }
    }
}

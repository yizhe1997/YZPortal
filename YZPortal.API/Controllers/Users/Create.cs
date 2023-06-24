using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Net;
using YZPortal.API.Infrastructure.Mediatr;
using YZPortal.Core.Domain.Contexts;
using YZPortal.Core.Domain.Database;
using YZPortal.Core.Domain.Database.Users;
using YZPortal.Core.Error;
using YZPortal.FullStackCore.Models.Users;

namespace YZPortal.API.Controllers.Users
{
    public class Create
    {
        public class Request : IRequest<Model>
        {
        }
        public class Model : UserModel
        {
        }
        public class RequestHandler : BaseRequestHandler<Request, Model>
        {
            UserManager<User> UserManager { get; }
            DatabaseOptions StorageOptions { get; set; }
            public RequestHandler(PortalContext dbContext, IMapper mapper, IHttpContextAccessor httpContext, CurrentContext userAccessor, UserManager<User> userManager, IOptions<DatabaseOptions> options) : base(dbContext, mapper, httpContext, userAccessor)
            {
                UserManager = userManager;
                StorageOptions = options.Value;
            }
            public override async Task<Model> Handle(Request request, CancellationToken cancellationToken)
            {
                // Check if user with current context sub id already exist
                var checkUser = await Database.Users.FirstOrDefaultAsync(u => u.SubjectIdentifier == CurrentContext.NameIdentifier);
                if (checkUser != null)
                    throw new RestException(HttpStatusCode.BadRequest, "User already exist!");

                // Create new user via current context
                var user = new User()
                {
                    SubjectIdentifier = CurrentContext.NameIdentifier,
                    DisplayName = CurrentContext.DisplayName,
                    AuthTime = CurrentContext.AuthTime,
                    LastidpAccessToken = CurrentContext.IdpAccessToken,
                    AuthExpireTime = CurrentContext.AuthExpireTime,
                    AuthClassRef = CurrentContext.AuthClassRef,
                    IpAddress = CurrentContext.IpAddress,
                    Email = CurrentContext.Email,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = CurrentContext.NameIdentifier.ToString(),
                };

                // Create user and validate
                var createUserResult = await UserManager.CreateAsync(user);
                if (!createUserResult.Succeeded)
                    throw new RestException(HttpStatusCode.BadRequest, createUserResult.Errors.Select(e => e.Description).ToList());

                return Mapper.Map<Model>(user);
            }
        }
    }
}

using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;
using YZPortal.API.Infrastructure.Mediatr;
using YZPortal.API.Infrastructure.Security.Jwt;
using YZPortal.Core.Domain.Contexts;
using YZPortal.Core.Domain.Database.Users;
using YZPortal.Core.Error;

namespace YZPortal.API.Controllers.Users.Authenticate
{
    public class Create
    {
        public class Request : IRequest<Model>
        {
            public string? Email { get; set; }
            public string? Password { get; set; }
        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.Email).NotNull().NotEmpty().EmailAddress();
                RuleFor(x => x.Password).NotNull().NotEmpty();
            }
        }

        public class Model
        {
            public string? AuthToken { get; set; }
        }

        public class RequestHandler : BaseRequestHandler<Request, Model>
        {
            JwtTokenGenerator JwtTokenGenerator { get; }
            SignInManager<User> SignInManager { get; }

            public RequestHandler(PortalContext dbContext, IMapper mapper, IHttpContextAccessor httpContext, CurrentContext userAccessor, SignInManager<User> signInManager, JwtTokenGenerator jwtTokenGenerator) : base(dbContext, mapper, httpContext, userAccessor)
            {
                JwtTokenGenerator = jwtTokenGenerator;
                SignInManager = signInManager;
            }
            public override async Task<Model> Handle(Request request, CancellationToken cancellationToken)
            {
                var details = "Incorrect username & password combination.";

                var result = await SignInManager.PasswordSignInAsync(request.Email, request.Password, false, false);
                if (result.Succeeded)
                {
                    var user = await Database.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
                    if (user == null)
                        throw new RestException(HttpStatusCode.Unauthorized, details);

                    user.LastLoggedIn = DateTime.UtcNow;

                    await Database.SaveChangesAsync();

                    return new Model{ AuthToken = await JwtTokenGenerator.CreateToken(user.Id.ToString()) };
                }

                if (result.IsLockedOut)
                {
                    details = "User locked out.";
                }
                else if (result.IsNotAllowed)
                {
                    details = "User not allowed to authenticate.";
                }
                else if (result.RequiresTwoFactor)
                {
                    details = "Two-factor authentication required.";
                }

                throw new RestException(HttpStatusCode.Unauthorized, details);
            }
        }
    }
}
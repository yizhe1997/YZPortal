using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace YZPortal.API.Controllers.Users.ResetPassword
{
    public class Claim
    {
        public class Request : IRequest<Model>
        {
            public Guid Token { get; set; }
            public string Password { get; set; }
        }

        public class Model
        {
            public string AuthToken { get; set; }
        }

        public class RequestHandler : BaseRequestHandler<Request, Model>
        {
            UserManager<User> UserManager { get; }
            JwtTokenGenerator JwtTokenGenerator { get; }

            public RequestHandler(DealerPortalContext dbContext, FunctionApiContext apiContext, IMapper mapper, IHttpContextAccessor httpContext, CurrentUserContext userAccessor, UserManager<User> userManager, JwtTokenGenerator jwtTokenGenerator) : base(dbContext, apiContext, mapper, httpContext, userAccessor)
            {
                UserManager = userManager;
                JwtTokenGenerator = jwtTokenGenerator;
            }

            public override async Task<Model> Handle(Request request, CancellationToken cancellationToken)
            {
                // We use token as Id here since we don't want to send the actual reset token through email
                var passwordReset = await Database.PasswordResets.FirstOrDefaultAsync(pr => pr.Token == request.Token && pr.Claimed == null && pr.ValidUntil > DateTime.UtcNow);

                if (passwordReset == null) throw new RestException(HttpStatusCode.NotFound, "Password reset request not found or expired.");

                var user = await Database.Users.FirstOrDefaultAsync(u => u.Id == passwordReset.UserId);

                if (user == null) throw new RestException(HttpStatusCode.NotFound, "User not found.");

                if (user.SecurityStamp == null)
                {
                    user.SecurityStamp = Guid.NewGuid().ToString();
                }

                var token = await UserManager.GeneratePasswordResetTokenAsync(user);
                var result = await UserManager.ResetPasswordAsync(user, token, request.Password);
                var jwtToken = await JwtTokenGenerator.CreateToken(user.Id.ToString());

                if (result.Succeeded)
                {
                    passwordReset.Claimed = DateTime.UtcNow;
                    Database.PasswordResets.Update(passwordReset);
                    await Database.SaveChangesAsync();
                    return new Model { AuthToken = jwtToken };
                }
                throw new RestException(HttpStatusCode.BadRequest, result.Errors.Select(e => e.Description).ToList());
            }
        }
    }
}

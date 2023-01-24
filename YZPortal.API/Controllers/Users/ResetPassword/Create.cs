using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace YZPortal.API.Controllers.Users.ResetPassword
{
    public static class Create
    {
        public class Request : IRequest<Model>
        {
            [Required]
            public string Email { get; set; }
            public string CallbackUrl { get; set; } = "{0}";
        }

        //public class Validator : AbstractValidator<Request>
        //{
        //    public Validator()
        //    {
        //        RuleFor(c => c.Email).NotNull().NotEmpty().EmailAddress();
        //    }
        //}

        public class Model
        {
            public string CallbackUrl { get; set; }
        }

        internal class RequestHandler : BaseRequestHandler<Request, Model>
        {
            IWebHostEnvironment Environment { get; }

            public RequestHandler(DealerPortalContext dbContext, FunctionApiContext apiContext, IMapper mapper, IHttpContextAccessor httpContext, CurrentUserContext userAccessor, IWebHostEnvironment hostingEnvironment) : base(dbContext, apiContext, mapper, httpContext, userAccessor)
            {
                Environment = hostingEnvironment;
            }

            public override async Task<Model> Handle(Request request, CancellationToken cancellationToken)
            {
                var user = await Database.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

                if (user == null) throw new RestException(HttpStatusCode.NotFound, "User not found.");

                var passwordReset = new PasswordReset { Id = Guid.NewGuid(), Email = request.Email, UserId = user.Id, ValidUntil = DateTime.UtcNow + TimeSpan.FromDays(3), CallbackUrl = request.CallbackUrl };

                passwordReset.CallbackUrl = string.Format(passwordReset.CallbackUrl, passwordReset.Token);

                Database.PasswordResets.Add(passwordReset);
                await Database.SaveChangesAsync();

                if (Environment.EnvironmentName.ToLower() == "development")
                {
                    // It is a security hazard to return this so only in dev mode.
                    return Mapper.Map<Model>(passwordReset);
                }

                return null;
            }
        }
    }
}

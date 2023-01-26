using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace YZPortal.API.Controllers.Memberships.Invites
{
    public class ValidateToken
    {
        public class Request : IRequest<Model>
        {
            [Required]
            public Guid Token { get; set; }

        }
        public class Model
        {
        }
        public class RequestHandler : BaseRequestHandler<Request, Model>
        {
            public RequestHandler(DealerPortalContext dbContext, FunctionApiContext apiContext, IMapper mapper, IHttpContextAccessor httpContext, CurrentUserContext userAccessor) : base(dbContext, apiContext, mapper, httpContext, userAccessor)
            {
            }
            public override async Task<Model> Handle(Request request, CancellationToken cancellationToken)
            {
                var invite = await Database.Invites.FirstOrDefaultAsync(i => i.Token == request.Token);

                if (invite == null)
                    throw new RestException(HttpStatusCode.NotFound, ":Invitation not found. Please contact admin!");

                if (invite.Claimed != null)
                    throw new RestException(HttpStatusCode.Conflict, ":Invitation has already been claimed on - " + invite.Claimed);

                if (invite.ValidUntil != null)
                    if (invite.ValidUntil > DateTime.UtcNow)
                        throw new RestException(HttpStatusCode.Conflict, ":Invitation expired on - " + invite.ValidUntil);

                return new Model { };
            }
        }
    }
}

using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;
using YZPortal.API.Infrastructure.Mediatr;
using YZPortal.Core.Domain.Contexts;
using YZPortal.Core.Error;

namespace YZPortal.API.Controllers.Memberships.Invites
{
    public class Delete
    {
        public class Request : IRequest<Model>
        {
            public Guid Id { get; set; }
        }
        public class Model : InviteViewModel
        {
        }
        public class RequestHandler : BaseRequestHandler<Request, Model>
        {
            public RequestHandler(PortalContext dbContext, IMapper mapper, IHttpContextAccessor httpContext, CurrentContext userAccessor) : base(dbContext, mapper, httpContext, userAccessor)
            {
            }
            public override async Task<Model> Handle(Request request, CancellationToken cancellationToken)
            {
                var invite = await CurrentContext.CurrentDealerInvites.FirstOrDefaultAsync(dr => dr.Id == request.Id);
                if (invite == null) 
                    throw new RestException(HttpStatusCode.NoContent, "Invite not found.");

                Database.MembershipInvites.Remove(invite);
                await Database.SaveChangesAsync();

                return Mapper.Map<Model>(invite);
            }
        }
    }
}

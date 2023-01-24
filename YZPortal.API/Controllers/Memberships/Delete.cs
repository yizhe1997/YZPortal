using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;
using YZPortal.Core.Domain.Contexts;

namespace YZPortal.Api.Controllers.Memberships
{
    public class Delete
    {
        public class Request : IRequest<Model>
        {
            public Guid Id { get; set; }
        }

        public class Model : MembershipsViewModel
        {
        }

        public class RequestHandler : BaseRequestHandler<Request, Model>
        {
            public RequestHandler(PortalContext dbContext, IMapper mapper, IHttpContextAccessor httpContext, CurrentContext userAccessor) : base(dbContext, mapper, httpContext, userAccessor)
            {
            }

            public override async Task<Model> Handle(Request request, CancellationToken cancellationToken)
            {
                var membership = await CurrentContext.DealerMemberships
                    .Include(m => m.User)
                    .Include(m => m.MembershipDealerRole)
                    .ThenInclude(m => m.DealerRole)
                    .Include(m => m.MembershipContentAccessLevels)
                    .ThenInclude(m => m.ContentAccessLevel)
                    .FirstOrDefaultAsync(m => m.Id == request.Id);

                if (membership == null) 
                    throw new RestException(HttpStatusCode.NotFound);

                // Make sure we are not deleting the last Admin
                //var lastAdminMembership = await CurrentContext.DealerMemberships
                //    .Include(m => m.MembershipDealerRoles)
                //    .ThenInclude(m => m.DealerRole)
                //    .Where(m => m.Id != request.Id)
                //    .Where(m => m.MembershipDealerRoles.DealerRole.Name.Contains(DealerRoleTypes.Admin))
                //    .FirstOrDefaultAsync();

                //if (lastAdminMembership == null)
                //    throw new RestException(HttpStatusCode.BadRequest, "Can't delete the last Admin member");

                Database.Memberships.Remove(membership);
                await Database.SaveChangesAsync();

                return Mapper.Map<Model>(membership);
            }
        }
    }
}

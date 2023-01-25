using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;
using YZPortal.Core.Domain.Contexts;

namespace YZPortal.Api.Controllers.Memberships
{
    public class Update
    {
        public class Request : IRequest<Model>
        {
            internal Guid Id { get; set; }
            public bool MasterAdmin { get; set; } = true;

            public string Roles { get; set; } 
            public List<string> AccessLevels { get; set; } = new List<string> { };
        }

        public class Model : MembershipsViewModel
        {
        }
        public class CommandHandler : BaseRequestHandler<Request, Model>
        {
            public CommandHandler(PortalContext dbContext, IMapper mapper, IHttpContextAccessor httpContext, CurrentContext userAccessor) : base(dbContext, mapper, httpContext, userAccessor)
            {
            }
            public override async Task<Model> Handle(Request request, CancellationToken cancellationToken)
            {
                var membership = await CurrentContext.CurrentDealerMemberships
                    .Include(x => x.MembershipDealerRole)
                    .Include(x => x.MembershipContentAccessLevels)
                    .FirstOrDefaultAsync(m => m.Id == request.Id);

                if (membership == null) throw new RestException(HttpStatusCode.NotFound);

                membership.UpdateRolesAndContentAccessLevels(Database, request.Roles, request.AccessLevels);
                
                Database.Memberships.Update(membership);
                
                await Database.SaveChangesAsync();

                // Create and Update Master Admin                
                if (membership.Admin != request.MasterAdmin)
                {
                    var dealerList = await Database.Dealers.ToListAsync();
                    var membershipList = await Database.Memberships.Where(x => x.UserId == membership.UserId).ToListAsync();
                    MembershipsHelper.CreateUpdateMasterAdmin(membershipList, dealerList, membership.UserId, request.MasterAdmin, Database);
                }

                var membershipResponse = await CurrentContext.CurrentDealerMemberships
                    .Include(x => x.MembershipDealerRole)
                    .Include(x => x.MembershipContentAccessLevels)
                    .FirstOrDefaultAsync(m => m.Id == request.Id);

                return Mapper.Map<Model>(membershipResponse);
            }
        }
    }
}

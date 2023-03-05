using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;
using YZPortal.API.Controllers.Memberships;
using YZPortal.API.Infrastructure.Mediatr;
using YZPortal.Core.Domain.Contexts;
using YZPortal.Core.Error;

namespace YZPortal.Api.Controllers.Memberships
{
    public class Update
    {
        public class Request : IRequest<Model>
        {
            internal Guid Id { get; set; }
            public bool MasterAdmin { get; set; } = true;

            public int Role { get; set; } 
            public List<int> ContentAccessLevels { get; set; } = new List<int>();
        }

        public class Model : MembershipViewModel
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

                membership.UpdateRolesAndContentAccessLevels(Database, request.Role, request.ContentAccessLevels);
                
                await Database.SaveChangesAsync();

                //// Create and Update Master Admin                
                //if (membership.Admin != request.MasterAdmin)
                //{
                //    var dealerList = await Database.Dealers.ToListAsync();
                //    var membershipList = await Database.Memberships.Where(x => x.UserId == membership.UserId).ToListAsync();
                //    MembershipsHelper.CreateUpdateMasterAdmin(membershipList, dealerList, membership.UserId, request.MasterAdmin, Database);
                //}

                var membershipResponse = await CurrentContext.CurrentDealerMemberships
                    .Include(x => x.MembershipDealerRole)
                    .Include(x => x.MembershipContentAccessLevels)
                    .FirstOrDefaultAsync(m => m.Id == request.Id);

                return Mapper.Map<Model>(membershipResponse);
            }
        }
    }
}

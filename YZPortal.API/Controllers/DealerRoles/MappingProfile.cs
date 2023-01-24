using AutoMapper;
using YZPortal.Core.Domain.Database.Memberships;

namespace YZPortal.Api.Controllers.DealerRoles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Index
            CreateMap<DealerRole, Index.Model>();
        }
    }
}

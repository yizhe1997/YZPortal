using AutoMapper;
using YZPortal.Core.Domain.Database.Memberships;

namespace YZPortal.API.Controllers.Memberships.DealerRoles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Index
            CreateMap<DealerRole, Index.Model>()
                .ForMember(c => c.Name, opt => opt.MapFrom(src => ((DealerRoleNames)src.Name).ToString()))
                .ForMember(c => c.Code, opt => opt.MapFrom(src => src.Name));
        }
    }
}

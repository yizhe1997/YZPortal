using AutoMapper;
using YZPortal.Core.Domain.Database.Memberships;

namespace YZPortal.Api.Controllers.ContentAccessLevels
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Index
            CreateMap<ContentAccessLevel, Index.Model>()
                .ForMember(c => c.Name, opt => opt.MapFrom(src => ((ContentAccessLevelNames)src.Name).ToString()))
                .ForMember(c => c.Code, opt => opt.MapFrom(src => src.Name));
        }
    }
}

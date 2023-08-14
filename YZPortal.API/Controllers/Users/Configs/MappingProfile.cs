using AutoMapper;
using YZPortal.Core.Domain.Database.EntityTypes.Users.Configs;
using YZPortal.FullStackCore.Models.Users.Configs;
using YZPortal.FullStackCore.Requests.Users.Configs;

namespace YZPortal.API.Controllers.Users.Configs
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UpdatePortalConfigRequest, PortalConfig>();
            CreateMap<PortalConfig, PortalConfigModel>();
            CreateMap<Tuple<PortalConfig>, ConfigsModel>()
                .ForMember(dest => dest.PortalConfigModel, opt => opt.MapFrom(source => source.Item1));
        }
    }
}

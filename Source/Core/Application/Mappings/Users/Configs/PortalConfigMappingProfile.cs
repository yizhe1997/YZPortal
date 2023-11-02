using Application.Features.Users.Configs.Commands.UpdatePortalConfig;
using Application.Features.Users.Configs.Queries.GetConfigs;
using AutoMapper;
using Domain.Entities.Users.Configs;

namespace Application.Mappings.Users.Configs
{
    public class PortalConfigMappingProfile : Profile
    {
        public PortalConfigMappingProfile()
        {
            CreateMap<UpdateUserPortalConfigCommand, PortalConfig>(MemberList.Source)
                .ForSourceMember(x => x.UserSubId, opt => opt.DoNotValidate())
                .ForMember(x => x.User, opt => opt.Ignore())
                .ForMember(x => x.UserSubjectIdentifier, opt => opt.Ignore());
            CreateMap<PortalConfig, PortalConfigDto>();
        }
    }
}

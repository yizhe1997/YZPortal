using AutoMapper;
using YZPortal.Core.Domain.Contexts;
using YZPortal.Core.Domain.Database.EntityTypes.Users;
using YZPortal.FullStackCore.Models.Users;
using YZPortal.FullStackCore.Requests.Users;

namespace YZPortal.API.Controllers.Users
{
    public class MappingProfile : Profile
	{
		public MappingProfile()
		{
            CreateMap<CurrentContext, User>()
                .ForMember(dest => dest.SubjectIdentifier, opt => opt.MapFrom(src => src.NameIdentifier))
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.DisplayName))
                .ForMember(dest => dest.AuthTime, opt => opt.MapFrom(src => src.AuthTime))
                .ForMember(dest => dest.LastidpAccessToken, opt => opt.MapFrom(src => src.IdpAccessToken))
                .ForMember(dest => dest.IdentityProvider, opt => opt.MapFrom(src => src.IdentityProvider))
                .ForMember(dest => dest.Identities, opt => opt.MapFrom(src => src.Identities))
                .ForMember(dest => dest.AuthExpireTime, opt => opt.MapFrom(src => src.AuthExpireTime))
                .ForMember(dest => dest.AuthClassRef, opt => opt.MapFrom(src => src.AuthClassRef))
                .ForMember(dest => dest.IpAddress, opt => opt.MapFrom(src => src.IpAddress))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email ?? src.NameIdentifier))
                // If new user or id is guid empty then map created date and created by with latest info, else map itself
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom((src, dest) => dest.Id == Guid.Empty ? DateTime.UtcNow : dest.CreatedDate))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom((src, dest) => dest.Id == Guid.Empty ? src.NameIdentifier : dest.CreatedBy))
                // If new user or id is guid empty then map updated date and updated by with itself, else with latest info
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom((src, dest) => dest.Id == Guid.Empty ? dest.UpdatedDate : DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom((src, dest) => dest.Id == Guid.Empty ? dest.UpdatedBy : src.NameIdentifier));
            CreateMap<Identity, IdentityModel>();
            CreateMap<User, UserModel>();
            CreateMap<UpdateUserRequest, Microsoft.Graph.Models.User>()
                .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.GivenName, opt => opt.MapFrom(src => src.FirstName));
            CreateMap<UpdateUserRequest, User>()
                .ForMember(dest => dest.SubjectIdentifier, opt => opt.Ignore());
        }
	}
}

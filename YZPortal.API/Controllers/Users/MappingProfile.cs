using AutoMapper;
using YZPortal.Core.Domain.Contexts;
using YZPortal.Core.Domain.Database.Users;
using YZPortal.FullStackCore.Models.Users;
using YZPortal.FullStackCore.Requests.Users;

namespace YZPortal.API.Controllers.Users
{
    public class MappingProfile : Profile
	{
		public MappingProfile()
		{
            CreateMap<CurrentContext, User>()
                .ForMember(c => c.SubjectIdentifier, opt => opt.MapFrom(src => src.NameIdentifier))
                .ForMember(c => c.DisplayName, opt => opt.MapFrom(src => src.DisplayName))
                .ForMember(c => c.AuthTime, opt => opt.MapFrom(src => src.AuthTime))
                .ForMember(c => c.LastidpAccessToken, opt => opt.MapFrom(src => src.IdpAccessToken))
                .ForMember(c => c.IdentityProvider, opt => opt.MapFrom(src => src.IdentityProvider))
                .ForMember(c => c.Identities, opt => opt.MapFrom(src => src.Identities))
                .ForMember(c => c.AuthExpireTime, opt => opt.MapFrom(src => src.AuthExpireTime))
                .ForMember(c => c.AuthClassRef, opt => opt.MapFrom(src => src.AuthClassRef))
                .ForMember(c => c.IpAddress, opt => opt.MapFrom(src => src.IpAddress))
                .ForMember(c => c.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(c => c.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(c => c.Email, opt => opt.MapFrom(src => src.Email ?? src.NameIdentifier))
                // If new user or id is guid empty then map created date and created by with latest info, else map itself
                .ForMember(c => c.CreatedDate, opt => opt.MapFrom((src, dest) => dest.Id == Guid.Empty ? DateTime.UtcNow : dest.CreatedDate))
                .ForMember(c => c.CreatedBy, opt => opt.MapFrom((src, dest) => dest.Id == Guid.Empty ? src.NameIdentifier : dest.CreatedBy))
                // If new user or id is guid empty then map updated date and updated by with itself, else with latest info
                .ForMember(c => c.UpdatedDate, opt => opt.MapFrom((src, dest) => dest.Id == Guid.Empty ? dest.UpdatedDate : DateTime.UtcNow))
                .ForMember(c => c.UpdatedBy, opt => opt.MapFrom((src, dest) => dest.Id == Guid.Empty ? dest.UpdatedBy : src.NameIdentifier));
            CreateMap<Identity, IdentityModel>();
            CreateMap<User, UserModel>();
            CreateMap<UpdateUserRequest, Microsoft.Graph.Models.User>()
                .ForMember(c => c.Surname, opt => opt.MapFrom(src => src.LastName))
                .ForMember(c => c.GivenName, opt => opt.MapFrom(src => src.FirstName));
            CreateMap<UpdateUserRequest, User>()
                .ForMember(x => x.SubjectIdentifier, opt => opt.Ignore());
        }
	}
}

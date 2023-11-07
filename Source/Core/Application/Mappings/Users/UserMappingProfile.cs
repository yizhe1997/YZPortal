using Application.Interfaces.Services.Identity;
using Application.Models.Identity;
using Application.Requests.Users;
using AutoMapper;
using Domain.Entities.Users;

namespace Application.Mappings.Users
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<ICurrentUserService, User>(MemberList.Source)
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
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                // If new user or id is guid empty then map created date and created by with latest info, else map itself
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom((src, dest) => dest.Id == Guid.Empty ? DateTime.UtcNow : dest.CreatedDate)) // TODO: remove this? the dbcontext handling already
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom((src, dest) => dest.Id == Guid.Empty ? src.NameIdentifier : dest.CreatedBy))
                // If new user or id is guid empty then map updated date and updated by with itself, else with latest info
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom((src, dest) => dest.Id == Guid.Empty ? dest.UpdatedDate : DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom((src, dest) => dest.Id == Guid.Empty ? dest.UpdatedBy : src.NameIdentifier));
            CreateMap<Identity, IdentityModel>();
            CreateMap<User, UserModel>();
            CreateMap<UpdateUserCommand, User>(MemberList.Source)
                .ForMember(dest => dest.SubjectIdentifier, opt => opt.Ignore());
        }
    }
}
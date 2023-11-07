using Application.Models.Graph;
using Application.Requests.Users;
using AutoMapper;
using Microsoft.Graph.Models;

namespace Infrastructure.Mappings
{
    public class GraphUserMappingProfile : Profile
    {
        public GraphUserMappingProfile() 
        {
            CreateMap<UpdateUserCommand, Microsoft.Graph.Models.User>()
                .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.GivenName, opt => opt.MapFrom(src => src.FirstName));
            CreateMap<User, UserModel>();
        }
    }
}

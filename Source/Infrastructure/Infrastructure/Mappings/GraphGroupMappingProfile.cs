using Application.Models.Graph;
using AutoMapper;
using Microsoft.Graph.Models;

namespace Infrastructure.Mappings
{
    public class GraphGroupMappingProfile : Profile
    {
        public GraphGroupMappingProfile()
        {
            // Index
            CreateMap<Group, GroupModel>();
        }
    }
}

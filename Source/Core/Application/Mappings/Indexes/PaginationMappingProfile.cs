using Application.Models;
using AutoMapper;

namespace Application.Mappings.Indexes
{
    public class PaginationMappingProfile : Profile
    {
        public PaginationMappingProfile()
        {
            CreateMap(typeof(PaginatedResult<>), typeof(PaginatedResult<>), MemberList.Source);
        }
    }
}

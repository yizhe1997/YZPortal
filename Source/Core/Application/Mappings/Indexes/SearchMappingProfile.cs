using Application.Models;
using AutoMapper;

namespace Application.Mappings.Indexes
{
    public class SearchMappingProfile : Profile
    {
        public SearchMappingProfile()
        {
            CreateMap(typeof(SearchResult<>), typeof(SearchResult<>), MemberList.Source);
        }
    }
}

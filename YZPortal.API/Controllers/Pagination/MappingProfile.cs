using AutoMapper;
using YZPortal.API.Infrastructure.Mediatr;

namespace YZPortal.API.Controllers.Pagination
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Search Response
            CreateMap(typeof(SearchRequest<>), typeof(SearchResponse<>), MemberList.Source);
            CreateMap(typeof(PaginatedList<>), typeof(SearchResponse<>), MemberList.Source);

            // Paged Response
            CreateMap(typeof(PagedRequest<>), typeof(PagedResponse<>), MemberList.Source);
            CreateMap(typeof(PaginatedList<>), typeof(PagedResponse<>), MemberList.Source);
        }
    }
}

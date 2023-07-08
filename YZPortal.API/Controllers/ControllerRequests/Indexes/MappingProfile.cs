using AutoMapper;
using YZPortal.Core.Indexes;
using YZPortal.FullStackCore.Models.Abstracts;

namespace YZPortal.API.Controllers.ControllerRequests.Indexes
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Search Response
            CreateMap(typeof(SearchRequest<>), typeof(SearchModel<>), MemberList.Source);
            CreateMap(typeof(SearchList<>), typeof(SearchModel<>), MemberList.Source);

            // Paged Response
            CreateMap(typeof(PagedRequest<>), typeof(PagedModel<>), MemberList.Source);
            CreateMap(typeof(PaginatedList<>), typeof(PagedModel<>), MemberList.Source);
        }
    }
}

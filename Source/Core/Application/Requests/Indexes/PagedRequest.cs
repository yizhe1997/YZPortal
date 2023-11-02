using Application.Interfaces.Indexes;

namespace Application.Requests.Indexes
{
    public class PagedRequest : IPaginationParams
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int MaxPages { get; set; } = -1; // TODO: pass param to method
    }
}

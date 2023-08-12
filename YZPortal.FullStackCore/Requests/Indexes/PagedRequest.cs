namespace YZPortal.FullStackCore.Requests.Indexes
{
    public class PagedRequest : IPaginationParams
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}

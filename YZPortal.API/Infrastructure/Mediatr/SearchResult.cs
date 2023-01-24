namespace YZPortal.API.Infrastructure.Mediatr
{
    public class SearchResult
    {
        public string? SearchString { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public List<int> Pages { get; set; } = new List<int>();
    }
}

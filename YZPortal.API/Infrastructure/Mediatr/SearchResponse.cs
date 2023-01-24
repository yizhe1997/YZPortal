namespace YZPortal.API.Infrastructure.Mediatr
{
    public class SearchResponse<T> : PagedResponse<T>
    {
        public string? SearchString { get; set; }
    }
}

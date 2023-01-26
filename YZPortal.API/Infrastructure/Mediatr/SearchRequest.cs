namespace YZPortal.API.Infrastructure.Mediatr
{
    public abstract class SearchRequest<T> : PagedRequest<T>
    {
        public string SearchString { get; set; } = string.Empty;
        public string OrderBy { get; set; } = "id";
        public string Lang { get; set; } = "en";
        public string? CreatedBy { get; set; }

    }
}

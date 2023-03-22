namespace YZPortal.Client.Models.Abstracts
{
    public class PagedModel<T>
    {
        public string? SearchString { get; set; }
        public string? OrderBy { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; internal set; }
        public int TotalItems { get; internal set; }
        public List<T> Results { get; set; } = new List<T>();
    }
}

namespace YZPortal.FullStackCore.Models.Abstracts
{
    public class SearchModel<T> : PagedModel<T>
    {
        public string? SearchString { get; set; }
    }
}

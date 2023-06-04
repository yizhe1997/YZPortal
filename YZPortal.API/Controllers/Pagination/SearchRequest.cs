namespace YZPortal.API.Controllers.Pagination
{
    public abstract class SearchRequest<T> : PagedRequest<T>
    {
        public string SearchString { get; set; } = string.Empty;
        public virtual string OrderBy { get; set; } = "id";
        public string Lang { get; set; } = "en";
        public string? CreatedBy { get; set; }

        #region Graph

        public string[] OrderByArray { get; set; } = Array.Empty<string>();
        public string[] Select { get; set; } = Array.Empty<string>();

        #endregion
    }
}

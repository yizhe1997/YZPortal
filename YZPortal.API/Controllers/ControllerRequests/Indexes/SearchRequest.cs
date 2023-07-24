using YZPortal.Core.Indexes;

namespace YZPortal.API.Controllers.ControllerRequests.Indexes
{
    public class SearchRequest<T> : PagedRequest<T>, ISearchParams
    {
        public string SearchString { get; set; } = string.Empty;
        public string Lang { get; set; } = "en";
        public string[] OrderBy { get; set; } = Array.Empty<string>();
        public string[] Select { get; set; } = Array.Empty<string>();
    }
}

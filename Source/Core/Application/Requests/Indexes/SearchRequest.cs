using Application.Interfaces.Indexes;

namespace Application.Requests.Indexes
{
    public class SearchRequest : PagedRequest, ISearchParams
    {
        public string SearchString { get; set; } = string.Empty;
        public string Lang { get; set; } = "en";
        public string[] OrderBy { get; set; } = [];
        public string[] Select { get; set; } = [];
    }
}

using YZPortal.FullStackCore.Requests.Indexes;

namespace YZPortal.Core.Indexes
{
    public class SearchList<T> : PaginatedList<T>
    {
        public string SearchString { get; set; } = string.Empty;
        public string Lang { get; set; } = "en";
        public string[] OrderBy { get; set; } = Array.Empty<string>();
        public string[] Select { get; set; } = Array.Empty<string>();

        public SearchList(PaginatedList<T> paginatedList, ISearchParams searchParams) : base ()
        {
            // update object instance with all search properties required by the view
            SearchString = searchParams.SearchString;
            Lang = searchParams.Lang;
            OrderBy = searchParams.OrderBy;
            Select = searchParams.Select;

            // update object instance with all pager properties required by the view
            TotalItems = paginatedList.TotalItems;
            PageNumber = paginatedList.PageNumber;
            PageSize = paginatedList.PageSize;
            TotalPages = paginatedList.TotalPages;
            StartPage = paginatedList.StartPage;
            EndPage = paginatedList.EndPage;
            StartIndex = paginatedList.StartIndex;
            EndIndex = paginatedList.EndIndex;
            Pages = paginatedList.Pages;

            // Add items to the underlying collection
            AddRange(paginatedList.Results);
        }
    }
}

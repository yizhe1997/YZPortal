namespace YZPortal.Core.Indexes
{
    public class PaginatedList<T> : List<T>
    {
        public int TotalItems { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int StartPage { get; set; }
        public int EndPage { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public IEnumerable<int> Pages { get; set; } = Enumerable.Empty<int>();
        public List<T> Results { get { return this; } }

        public PaginatedList(List<T> items, int totalItems, int pageNumber, int pageSize = 10, int maxPages = -1)
        {
            // calculate total pages
            var totalPages = (int)Math.Ceiling(totalItems / (float)pageSize);
            totalPages = totalPages == 0 ? 1 : totalPages;

            // ensure current page isn't out of range
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }
            else if (pageNumber > totalPages)
            {
                pageNumber = totalPages;
            }

            int startPage, endPage;
            if (totalPages <= maxPages || maxPages < 0)
            {
                // total pages less than max or (infinite: -1) so show all pages
                startPage = 1;
                endPage = totalPages;
            }
            else
            {
                // total pages more than max so calculate start and end pages
                var maxPagesBeforeCurrentPage = (int)Math.Floor(maxPages / (float)2);
                var maxPagesAfterCurrentPage = (int)Math.Ceiling(maxPages / (float)2) - 1;
                if (pageNumber <= maxPagesBeforeCurrentPage)
                {
                    // current page near the start
                    startPage = 1;
                    endPage = maxPages;
                }
                else if (pageNumber + maxPagesAfterCurrentPage >= totalPages)
                {
                    // current page near the end
                    startPage = totalPages - maxPages + 1;
                    endPage = totalPages;
                }
                else
                {
                    // current page somewhere in the middle
                    startPage = pageNumber - maxPagesBeforeCurrentPage;
                    endPage = pageNumber + maxPagesAfterCurrentPage;
                }
            }

            // calculate start and end item indexes
            var startIndex = (pageNumber - 1) * pageSize;
            var endIndex = Math.Min(startIndex + pageSize - 1, totalItems - 1);

            // create an array of pages that can be looped over
            var pages = Enumerable.Range(startPage, endPage + 1 - startPage);

            // update object instance with all pager properties required by the view
            TotalItems = totalItems;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalPages = totalPages;
            StartPage = startPage;
            EndPage = endPage;
            StartIndex = startIndex;
            EndIndex = endIndex;
            Pages = pages;

            // Add items to the underlying collection
            AddRange(items);
        }

        public PaginatedList()
        {
        }
    }
}

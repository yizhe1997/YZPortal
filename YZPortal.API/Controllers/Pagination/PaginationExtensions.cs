using Microsoft.EntityFrameworkCore;

namespace YZPortal.API.Controllers.Pagination
{
    public static class PaginationExtensions
    {
        public static Task<PaginatedList<TDestination>> ToPaginatedListAsync<TDestination>(this IQueryable<TDestination> queryable, IPaginationParams paginationParams)
            => PaginatedList<TDestination>.CreateAsync(queryable, paginationParams.PageNumber, paginationParams.PageSize);

        public static Task<PaginatedList<TDestination>> ToPaginatedListAsync<TDestination>(this IQueryable<TDestination> queryable, int pageNumber, int pageSize)
            => PaginatedList<TDestination>.CreateAsync(queryable, pageNumber, pageSize);

        public static PaginatedList<TDestination> ToPaginatedList<TDestination>(this IQueryable<TDestination> queryable, int pageNumber, int pageSize)
            => PaginatedList<TDestination>.Create(queryable, pageNumber, pageSize);

        public static PaginatedList<TDestination> ToPaginatedList<TDestination>(this IQueryable<TDestination> queryable, IPaginationParams paginationParams)
            => PaginatedList<TDestination>.Create(queryable, paginationParams.PageNumber, paginationParams.PageSize);
    }

    public class PaginatedList<T> : List<T>
    {
        public int TotalItems { get; private set; }
        public int PageNumber { get; private set; }
        public int PageSize { get; private set; }
        public int TotalPages { get; private set; }
        public int StartPage { get; private set; }
        public int EndPage { get; private set; }
        public int StartIndex { get; private set; }
        public int EndIndex { get; private set; }
        public IEnumerable<int> Pages { get; private set; }
        public List<T> Results { get { return this; } }

        public PaginatedList(List<T> items, int totalItems, int pageNumber, int pageSize = 10, int maxPages = -1)
        {
            // calculate total pages
            var totalPages = (int)Math.Ceiling(totalItems / (float)pageSize);

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

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int currentPage, int pageSize, int maxPages = -1)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((currentPage - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginatedList<T>(items, count, currentPage, pageSize, maxPages);
        }

        public static PaginatedList<T> Create(IQueryable<T> source, int currentPage, int pageSize, int maxPages = -1)
        {
            var count = source.Count();
            var items = source.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            return new PaginatedList<T>(items, count, currentPage, pageSize, maxPages);
        }
    }

    public interface IPaginationParams
    {
        int PageNumber { get; set; }
        int PageSize { get; set; }
    }
}

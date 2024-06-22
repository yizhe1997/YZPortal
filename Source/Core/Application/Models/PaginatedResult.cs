using Application.Interfaces;
using Application.Interfaces.Indexes;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Application.Models
{
    public class PaginatedResult<T> : Result<T>, IPaginatedResult<T>
	{
        public int TotalItems { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
		//public int StartPage { get; set; }
		//public int EndPage { get; set; }
		//public int StartIndex { get; set; }
		//public int EndIndex { get; set; }
		//public IEnumerable<int> Pages { get; set; } = [];
        public new List<T> Data { get; set; } = [];

        public PaginatedResult(bool succeeded, IPaginationParams paginationParams, List<T> data, int totalItems)
        {
            // A reminder: https://enterprisecraftsmanship.com/posts/error-handling-exception-or-result/
            try
            {
                // calculate total pages
                var totalPages = (int)Math.Ceiling(totalItems / (float)paginationParams.PageSize);
                totalPages = totalPages == 0 ? 1 : totalPages;

                // ensure current page isn't out of range
                if (paginationParams.PageNumber < 1)
                {
                    paginationParams.PageNumber = 1;
                }
                else if (paginationParams.PageNumber > totalPages)
                {
                    paginationParams.PageNumber = totalPages;
                }

                int startPage, endPage;
                if (totalPages <= paginationParams.MaxPages || paginationParams.MaxPages < 0)
                {
                    // total pages less than max or (infinite: -1) so show all pages
                    startPage = 1;
                    endPage = totalPages;
                }
                else
                {
                    // total pages more than max so calculate start and end pages
                    var maxPagesBeforeCurrentPage = (int)Math.Floor(paginationParams.MaxPages / (float)2);
                    var maxPagesAfterCurrentPage = (int)Math.Ceiling(paginationParams.MaxPages / (float)2) - 1;
                    if (paginationParams.PageNumber <= maxPagesBeforeCurrentPage)
                    {
                        // current page near the start
                        startPage = 1;
                        endPage = paginationParams.MaxPages;
                    }
                    else if (paginationParams.PageNumber + maxPagesAfterCurrentPage >= totalPages)
                    {
                        // current page near the end
                        startPage = totalPages - paginationParams.MaxPages + 1;
                        endPage = totalPages;
                    }
                    else
                    {
                        // current page somewhere in the middle
                        startPage = paginationParams.PageNumber - maxPagesBeforeCurrentPage;
                        endPage = paginationParams.PageNumber + maxPagesAfterCurrentPage;
                    }
                }

                // calculate start and end item indexes
                var startIndex = (paginationParams.PageNumber - 1) * paginationParams.PageSize;
                var endIndex = Math.Min(startIndex + paginationParams.PageSize - 1, totalItems - 1);

                // create an array of pages that can be looped over
                var pages = Enumerable.Range(startPage, endPage + 1 - startPage);

                // update object instance with all page properties required by the view
                TotalItems = totalItems;
                PageNumber = paginationParams.PageNumber;
                PageSize = paginationParams.PageSize;
                TotalPages = totalPages;
                //StartPage = startPage;
                //EndPage = endPage;
                //StartIndex = startIndex;
                //EndIndex = endIndex;
                //Pages = pages;

                // Add items to the underlying collection
                Data = data;

                // Base
                Succeeded = succeeded;
            }
            catch 
            {
                // update object instance with all page properties required by the view
                PageNumber = 1;
                PageSize = paginationParams.PageSize;

                // Add items to the underlying collection
                Data = [];

                // Base
                Succeeded = false;
            }
        }

        // Ref: https://stackoverflow.com/questions/60806455/i-keep-getting-needs-to-have-a-constructor-with-0-args-or-only-optional-args
        public PaginatedResult()
        {
        }

        #region Async Methods

        #region Success Methods

        public static async Task<PaginatedResult<T>> SuccessAsync(IPaginationParams paginationParams, IQueryable<T> query, CancellationToken cancellationToken = default)
        {
            var count = await query.CountAsync(cancellationToken);
            var items = await query.Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize).Take(paginationParams.PageSize).ToListAsync(cancellationToken);

            return new PaginatedResult<T>(true, paginationParams, items, count);
        }

        public static async Task<PaginatedResult<TModel>> SuccessAsync<TModel>(IPaginationParams paginationParams, IQueryable<T> query, IMapper mapper, CancellationToken cancellationToken = default)
        {
            var paginatedResult = await SuccessAsync(paginationParams, query, cancellationToken);
            var mappedPaginatedResult = mapper.Map<PaginatedResult<TModel>>(paginatedResult);
            return mappedPaginatedResult;
        }

        public static async Task<PaginatedResult<T>> SuccessAsync(IPaginationParams paginationParams, List<T> data, CancellationToken cancellationToken = default) =>
            await SuccessAsync(paginationParams, data.AsQueryable(), cancellationToken);

        public static async Task<PaginatedResult<TModel>> SuccessAsync<TModel>(IPaginationParams paginationParams, List<T> data, IMapper mapper, CancellationToken cancellationToken = default)
        {
            var paginatedResult = await SuccessAsync(paginationParams, data, cancellationToken);
            var mappedPaginatedResult = mapper.Map<PaginatedResult<TModel>>(paginatedResult);
            return mappedPaginatedResult;
        }

        #endregion

        #region Failure Methods

        public async static Task<PaginatedResult<T>> FailAsync(IPaginationParams paginationParams) =>
            await Task.FromResult(Fail(paginationParams));

		public async static Task<PaginatedResult<T>> FailAsync(IPaginationParams paginationParams, string error) =>
			await Task.FromResult(Fail(paginationParams, error));

		public async static Task<PaginatedResult<T>> FailAsync(IPaginationParams paginationParams, List<string> errors) =>
			await Task.FromResult(Fail(paginationParams, errors));

		#endregion

		#endregion

		#region Non-Async Methods

		#region Success Methods

		public static PaginatedResult<T> Success(IPaginationParams paginationParams, IQueryable<T> query)
        {
            var count = query.Count();
            var items = query.Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize).Take(paginationParams.PageSize).ToList();

            return new PaginatedResult<T>(true, paginationParams, items, count);
        }

        public static PaginatedResult<TModel> Success<TModel>(IPaginationParams paginationParams, IQueryable<T> query, IMapper mapper)
        {
            var paginatedResult = Success(paginationParams, query);
            var mappedPaginatedResult = mapper.Map<PaginatedResult<TModel>>(paginatedResult);
            return mappedPaginatedResult;
        }

        public static PaginatedResult<T> Success(IPaginationParams paginationParams, List<T> data) =>
            Success(paginationParams, data.AsQueryable());

        public static PaginatedResult<TModel> Success<TModel>(IPaginationParams paginationParams, IMapper mapper, List<T> data)
        {
            var paginatedResult = Success(paginationParams, data.AsQueryable());
            var mappedPaginatedResult = mapper.Map<PaginatedResult<TModel>>(paginatedResult);
            return mappedPaginatedResult;
        }

        #endregion

        #region Failure Methods

        public static PaginatedResult<T> Fail(IPaginationParams paginationParams) =>
            new(false, paginationParams, [], 0);

		public static PaginatedResult<T> Fail(IPaginationParams paginationParams, string error) =>
			new(false, paginationParams, [], 0)
			{
				Errors = [error]
			};

		public static PaginatedResult<T> Fail(IPaginationParams paginationParams, List<string> errors) =>
			new(false, paginationParams, [], 0)
			{
				Errors = errors
			};

		#endregion

		#endregion
	}
}

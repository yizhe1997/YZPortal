using Application.Interfaces;
using Application.Interfaces.Indexes;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Application.Models
{
	public class SearchResult<T> : PaginatedResult<T>, ISearchResult<T>
	{
        public string SearchString { get; set; } = string.Empty;
        public string Lang { get; set; } = "en";
        public string[] OrderBy { get; set; } = [];
        public string[] Select { get; set; } = [];

        public SearchResult(bool succeeded, ISearchParams searchParams, List<T> data, int totalItems) : base(succeeded, searchParams, data, totalItems)
        {
            // update object instance with all search properties required by the view
            SearchString = searchParams.SearchString;
            Lang = searchParams.Lang;
            OrderBy = searchParams.OrderBy;
            Select = searchParams.Select;
        }

        // Ref: https://stackoverflow.com/questions/60806455/i-keep-getting-needs-to-have-a-constructor-with-0-args-or-only-optional-args
        public SearchResult()
        {
        }

        #region Async Methods

        #region Success Methods

        public static async Task<SearchResult<T>> SuccessAsync(ISearchParams searchParams, IQueryable<T> query, System.Linq.Expressions.Expression<Func<T, bool>>? searchPredicate = null, CancellationToken cancellationToken = default)
        {
            #region Query formulation

            // Chain search predicate to base query
            if (searchPredicate != null)
            {
                query = query.Where(searchPredicate);
            }

            // Get entity properties
            var valiedProperties = query.ElementType.GetProperties().Select(x => x.Name);

            // Declare string comparor
            var stringComparor = StringComparer.OrdinalIgnoreCase;

            // Chain the query params for order by if valid
            var validOrderByProperties = searchParams.OrderBy.Where(x => valiedProperties.Contains(x.Split(" ").First(), stringComparor));
            if (validOrderByProperties.Any())
            {
                query = query.OrderBy(string.Join(",", searchParams.OrderBy));
            }

            // Chain the query params for Select if valid
            var validSelectProperties = searchParams.Select.Where(x => valiedProperties.Contains(x.Split(" ").First(), stringComparor));
            if (validSelectProperties.Any())
            {
                query = query.Select<T>(string.Join(",", searchParams.Select));
            }

            #endregion

            // Return search list
            var count = await query.CountAsync(cancellationToken);
            var items = await query.Skip((searchParams.PageNumber - 1) * searchParams.PageSize).Take(searchParams.PageSize).ToListAsync(cancellationToken);

            return new SearchResult<T>(true, searchParams, items, count);
        }

        public static async Task<SearchResult<TModel>> SuccessAsync<TModel>(ISearchParams searchParams, IQueryable<T> query, IMapper mapper, System.Linq.Expressions.Expression<Func<T, bool>>? searchPredicate = null, CancellationToken cancellationToken = default)
        {
            var searchResult = await SuccessAsync(searchParams, query, searchPredicate, cancellationToken);
            var mappedSearchResult = mapper.Map<SearchResult<TModel>>(searchResult);
            return mappedSearchResult;
        }

        public static async Task<SearchResult<T>> SuccessAsync(ISearchParams searchParams, List<T> data, System.Linq.Expressions.Expression<Func<T, bool>>? searchPredicate = null, CancellationToken cancellationToken = default) =>
            await SuccessAsync(searchParams, data.AsQueryable(), searchPredicate, cancellationToken);

        public static async Task<SearchResult<TModel>> SuccessAsync<TModel>(ISearchParams searchParams, List<T> data, IMapper mapper, System.Linq.Expressions.Expression<Func<T, bool>>? searchPredicate = null, CancellationToken cancellationToken = default)
        {
            var searchResult = await SuccessAsync(searchParams, data, searchPredicate, cancellationToken);
            var mappedSearchResult = mapper.Map<SearchResult<TModel>>(searchResult);
            return mappedSearchResult;
        }

        #endregion

        #region Failure Methods

        public async static Task<SearchResult<T>> FailAsync(ISearchParams searchParams) =>
            await Task.FromResult(Fail(searchParams));

		public async static Task<SearchResult<T>> FailAsync(ISearchParams searchParams, string error) =>
			await Task.FromResult(Fail(searchParams, error));

		public async static Task<SearchResult<T>> FailAsync(ISearchParams searchParams, List<string> errors) =>
			await Task.FromResult(Fail(searchParams, errors));

		#endregion

		#endregion

		#region Non-Async Methods

		#region Success Methods

		public static SearchResult<T> Success(ISearchParams searchParams, IQueryable<T> query, System.Linq.Expressions.Expression<Func<T, bool>>? searchPredicate = null)
        {
            #region Query formulation

            // Chain search predicate to base query
            if (searchPredicate != null)
            {
                query = query.Where(searchPredicate);
            }

            // Get entity properties
            var valiedProperties = query.ElementType.GetProperties().Select(x => x.Name);

            // Declare string comparor
            var stringComparor = StringComparer.OrdinalIgnoreCase;

            // Chain the query params for order by if valid
            var validOrderByProperties = searchParams.OrderBy.Where(x => valiedProperties.Contains(x.Split(" ").First(), stringComparor));
            if (validOrderByProperties.Any())
            {
                query = query.OrderBy(string.Join(",", searchParams.OrderBy));
            }

            // Chain the query params for Select if valid
            var validSelectProperties = searchParams.Select.Where(x => valiedProperties.Contains(x.Split(" ").First(), stringComparor));
            if (validSelectProperties.Any())
            {
                query = query.Select<T>(string.Join(",", searchParams.Select));
            }

            #endregion

            // Return search list
            var count = query.Count();
            var items = query.Skip((searchParams.PageNumber - 1) * searchParams.PageSize).Take(searchParams.PageSize).ToList();

            return new SearchResult<T>(true, searchParams, items, count);
        }

        public static SearchResult<TModel> Success<TModel>(ISearchParams searchParams, IQueryable<T> query, IMapper mapper, System.Linq.Expressions.Expression<Func<T, bool>>? searchPredicate = null)
        {
            var searchResult = Success(searchParams, query, searchPredicate);
            var mappedSearchResult = mapper.Map<SearchResult<TModel>>(searchResult);
            return mappedSearchResult;
        }

        public static SearchResult<T> Success(ISearchParams searchParams, List<T> data, System.Linq.Expressions.Expression<Func<T, bool>>? searchPredicate = null) =>
            Success(searchParams, data.AsQueryable(), searchPredicate);

        public static SearchResult<TModel> Success<TModel>(ISearchParams searchParams, List<T> data, IMapper mapper, System.Linq.Expressions.Expression<Func<T, bool>>? searchPredicate = null)
        {
            var searchResult = Success(searchParams, data, searchPredicate);
            var mappedSearchResult = mapper.Map<SearchResult<TModel>>(searchResult);
            return mappedSearchResult;
        }

        #endregion

        #region Failure Methods

        public static SearchResult<T> Fail(ISearchParams searchParams) =>
            new(false, searchParams, [], 0);

		public static SearchResult<T> Fail(ISearchParams searchParams, string error) =>
			new(false, searchParams, [], 0)
            {
                Errors = [error]
            };

		public static SearchResult<T> Fail(ISearchParams searchParams, List<string> errors) =>
			new(false, searchParams, [], 0)
			{
				Errors = errors
			};

		#endregion

		#endregion
	}
}

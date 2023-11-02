using Application.Interfaces.Indexes;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Application.Models
{
    public class SearchResult<T> : PaginatedResult<T>
    {
        public string SearchString { get; set; } = string.Empty;
        public string Lang { get; set; } = "en";
        public string[] OrderBy { get; set; } = Array.Empty<string>();
        public string[] Select { get; set; } = Array.Empty<string>();

        public SearchResult(bool succeeded, ISearchParams searchParams, List<T> data, int totalItems, List<string> messages) : base(succeeded, searchParams, data, totalItems, messages)
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

        // TODO: fix the null
        #region Async Methods

        #region Success Methods

        public static async Task<SearchResult<T>> SuccessAsync(ISearchParams searchParams, IQueryable<T> query, System.Linq.Expressions.Expression<Func<T, bool>>? searchPredicate = null, List<string> messages = null, CancellationToken cancellationToken = new CancellationToken())
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

            return new SearchResult<T>(true, searchParams, items, count, messages);
        }

        public static async Task<SearchResult<TModel>> SuccessAsync<TModel>(ISearchParams searchParams, IQueryable<T> query, IMapper mapper, System.Linq.Expressions.Expression<Func<T, bool>>? searchPredicate = null, List<string> messages = null, CancellationToken cancellationToken = new CancellationToken())
        {
            var searchResult = await SuccessAsync(searchParams, query, searchPredicate, messages, cancellationToken);
            var mappedSearchResult = mapper.Map<SearchResult<TModel>>(searchResult);
            return mappedSearchResult;
        }

        public static async Task<SearchResult<T>> SuccessAsync(ISearchParams searchParams, List<T> data, System.Linq.Expressions.Expression<Func<T, bool>>? searchPredicate = null, List<string> messages = null, CancellationToken cancellationToken = new CancellationToken()) =>
            await SuccessAsync(searchParams, data.AsQueryable(), searchPredicate, messages, cancellationToken);

        public static async Task<SearchResult<TModel>> SuccessAsync<TModel>(ISearchParams searchParams, List<T> data, IMapper mapper, System.Linq.Expressions.Expression<Func<T, bool>>? searchPredicate = null, List<string> messages = null, CancellationToken cancellationToken = new CancellationToken())
        {
            var searchResult = await SuccessAsync(searchParams, data, searchPredicate, messages, cancellationToken);
            var mappedSearchResult = mapper.Map<SearchResult<TModel>>(searchResult);
            return mappedSearchResult;
        }

        #endregion

        #region Failure Methods

        public async static Task<SearchResult<T>> FailAsync(ISearchParams searchParams, List<string> messages = null) =>
            await Task.FromResult(Fail(searchParams, messages));

        #endregion

        #endregion

        #region Non-Async Methods

        #region Success Methods

        public static SearchResult<T> Success(ISearchParams searchParams, IQueryable<T> query, System.Linq.Expressions.Expression<Func<T, bool>>? searchPredicate = null, List<string> messages = null)
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

            return new SearchResult<T>(true, searchParams, items, count, messages);
        }

        public static SearchResult<TModel> Success<TModel>(ISearchParams searchParams, IQueryable<T> query, IMapper mapper, System.Linq.Expressions.Expression<Func<T, bool>>? searchPredicate = null, List<string> messages = null)
        {
            var searchResult = Success(searchParams, query, searchPredicate, messages);
            var mappedSearchResult = mapper.Map<SearchResult<TModel>>(searchResult);
            return mappedSearchResult;
        }

        public static SearchResult<T> Success(ISearchParams searchParams, List<T> data, System.Linq.Expressions.Expression<Func<T, bool>>? searchPredicate = null, List<string> messages = null) =>
            Success(searchParams, data.AsQueryable(), searchPredicate, messages);

        public static SearchResult<TModel> Success<TModel>(ISearchParams searchParams, List<T> data, IMapper mapper, System.Linq.Expressions.Expression<Func<T, bool>>? searchPredicate = null, List<string> messages = null)
        {
            var searchResult = Success(searchParams, data, searchPredicate, messages);
            var mappedSearchResult = mapper.Map<SearchResult<TModel>>(searchResult);
            return mappedSearchResult;
        }

        #endregion

        #region Failure Methods

        public static SearchResult<T> Fail(ISearchParams searchParams, List<string> messages = null) =>
            new(false, searchParams, new List<T>(), 0, messages);

        #endregion

        #endregion
    }
}

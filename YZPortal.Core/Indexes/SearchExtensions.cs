using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using YZPortal.FullStackCore.Requests.Indexes;

namespace YZPortal.Core.Indexes
{
    // TODO: fix the select param
    public static class SearchExtensions
    {
        public static async Task<SearchList<TEntity>> CreateSearchListAsync<TEntity>(this IQueryable<TEntity> query, ISearchParams searchParams, System.Linq.Expressions.Expression<Func<TEntity, bool>>? searchPredicate = null, CancellationToken cancellationToken = new CancellationToken()) where TEntity : class
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
                query = query.Select<TEntity>(string.Join(",", searchParams.Select));
            }

            #endregion

            // Return search list
            return new SearchList<TEntity>(await query.CreatePaginatedListAsync(searchParams, cancellationToken), searchParams);
        }

        public static SearchList<TEntity> CreateSearchList<TEntity>(this IQueryable<TEntity> query, ISearchParams searchParams, System.Linq.Expressions.Expression<Func<TEntity, bool>>? searchPredicate = null) where TEntity : class
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
                query = query.Select<TEntity>(string.Join(",", searchParams.Select));
            }

            #endregion

            // Return search list
            return new SearchList<TEntity>(query.CreatePaginatedList(searchParams), searchParams);
        }

        public static SearchList<TEntity> CreateSearchList<TEntity>(this List<TEntity> list, ISearchParams searchParams, System.Linq.Expressions.Expression<Func<TEntity, bool>>? searchPredicate = null) where TEntity : class
        {
            #region Query formulation

            var query = list.AsQueryable();

            // Chain search predicate to base query
            if (searchPredicate != null)
            {
                query = query.Where(searchPredicate);
            }

            // Get entiity properties
            var valiedProperties = typeof(TEntity).GetProperties().Select(x => x.Name);

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
                query = query.Select<TEntity>(string.Join(",", searchParams.Select));
            }

            #endregion

            // Return search list
            return new SearchList<TEntity>(query.CreatePaginatedList(searchParams), searchParams);
        }
    }
}

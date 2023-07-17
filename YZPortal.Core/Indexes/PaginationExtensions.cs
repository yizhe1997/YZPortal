using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;

namespace YZPortal.Core.Indexes
{
    public static class PaginationExtensions
    {
        // TODO: specify if field is queryable. maybe in warning specify the valid ones?
        public static async Task<PaginatedList<TEntity>> CreatePaginatedListAsync<TEntity>(this IQueryable<TEntity> query, IPaginationParams paginationParams, CancellationToken cancellationToken = new CancellationToken()) where TEntity : class
            => await query.ToPaginatedListAsync(paginationParams, cancellationToken);

        public static PaginatedList<TEntity> CreatePaginatedList<TEntity>(this IQueryable<TEntity> query, IPaginationParams paginationParams) where TEntity : class
            => query.ToPaginatedList(paginationParams);

        public static PaginatedList<TEntity> CreatePaginatedList<TEntity>(this List<TEntity> list, ISearchParams paginationParams) where TEntity : class
            => list.AsQueryable().ToPaginatedList(paginationParams);

        public static Task<PaginatedList<TEntity>> ToPaginatedListAsync<TEntity>(this IQueryable<TEntity> queryable, IPaginationParams paginationParams, CancellationToken cancellationToken = new CancellationToken())
            => queryable.CreateAsync(paginationParams, cancellationToken: cancellationToken);

        public static PaginatedList<TEntity> ToPaginatedList<TEntity>(this IQueryable<TEntity> queryable, IPaginationParams paginationParams)
            => queryable.Create(paginationParams);

        public static async Task<PaginatedList<TEntity>> CreateAsync<TEntity>(this IQueryable<TEntity> queryable, IPaginationParams paginationParams, int maxPages = -1, CancellationToken cancellationToken = new CancellationToken())
        {
            var pageSize = paginationParams.PageSize;
            var currentPage = paginationParams.PageNumber;
            var count = await queryable.CountAsync(cancellationToken);
            var items = await queryable.Skip((currentPage - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
            return new PaginatedList<TEntity>(items, count, currentPage, pageSize, maxPages);
        }

        public static PaginatedList<TEntity> Create<TEntity>(this IQueryable<TEntity> queryable, IPaginationParams paginationParams, int maxPages = -1)
        {
            var pageSize = paginationParams.PageSize;
            var currentPage = paginationParams.PageNumber;
            var count = queryable.Count();
            var items = queryable.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            return new PaginatedList<TEntity>(items, count, currentPage, pageSize, maxPages);
        }
    }
}

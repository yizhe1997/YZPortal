using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;

namespace YZPortal.Core.Indexes
{
    public static class PaginationExtensions
    {
        // TODO: specify if field is queryable. maybe in warning specify the valid ones?
        public static async Task<PaginatedList<TEntity>> CreatePaginatedListAsync<TEntity>(this IQueryable<TEntity> query, IPaginationParams paginationParams, CancellationToken cancellationToken = new CancellationToken()) where TEntity : class
            => await query.CreateAsync(paginationParams, cancellationToken: cancellationToken);

        public static PaginatedList<TEntity> CreatePaginatedList<TEntity>(this IQueryable<TEntity> query, IPaginationParams paginationParams) where TEntity : class
            => query.Create(paginationParams);

        public static PaginatedList<TEntity> CreatePaginatedList<TEntity>(this List<TEntity> list, IPaginationParams paginationParams) where TEntity : class
            => list.AsQueryable().Create(paginationParams);

        public static async Task<PaginatedList<TEntity>> CreateAsync<TEntity>(this IQueryable<TEntity> query, IPaginationParams paginationParams, int maxPages = -1, CancellationToken cancellationToken = new CancellationToken())
        {
            var pageSize = paginationParams.PageSize;
            var currentPage = paginationParams.PageNumber;
            var count = await query.CountAsync(cancellationToken);
            var items = await query.Skip((currentPage - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
            return new PaginatedList<TEntity>(items, count, currentPage, pageSize, maxPages);
        }

        public static PaginatedList<TEntity> Create<TEntity>(this IQueryable<TEntity> query, IPaginationParams paginationParams, int maxPages = -1)
        {
            var pageSize = paginationParams.PageSize;
            var currentPage = paginationParams.PageNumber;
            var count = query.Count();
            var items = query.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            return new PaginatedList<TEntity>(items, count, currentPage, pageSize, maxPages);
        }
    }
}

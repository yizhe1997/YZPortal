using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IGenericRepository<T, TId> where T : class, IEntity<TId>
    {
        IQueryable<T> Entities { get; }

        Task<T?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);
        Task<List<T>> GetByIdsAsync(List<TId> ids, CancellationToken cancellationToken = default);
        Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
        Task<List<T>> AddRangeAsync(List<T> entity, CancellationToken cancellationToken = default);
        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
        Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
        Task DeleteRangeAsync(List<T> entity, CancellationToken cancellationToken = default);
    }
}

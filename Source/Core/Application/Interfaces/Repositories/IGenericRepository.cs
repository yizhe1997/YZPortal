using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IGenericRepository<T, TId> where T : class, IEntity<TId>
    {
        IQueryable<T> Entities { get; }

        Task<T?> GetByIdAsync(TId id, CancellationToken cancellationToken = new CancellationToken());
        Task<List<T>> GetByIdsAsync(List<TId> ids, CancellationToken cancellationToken = new CancellationToken());
        Task<List<T>> GetAllAsync(CancellationToken cancellationToken = new CancellationToken());
        Task<T> AddAsync(T entity, CancellationToken cancellationToken = new CancellationToken());
        Task<List<T>> AddRangeAsync(List<T> entity, CancellationToken cancellationToken);
        Task UpdateAsync(T entity, CancellationToken cancellationToken = new CancellationToken());
        Task DeleteAsync(T entity, CancellationToken cancellationToken = new CancellationToken());
        Task DeleteRangeAsync(List<T> entity, CancellationToken cancellationToken);
    }
}

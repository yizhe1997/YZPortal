using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IGenericRepository<T, TId> where T : class, IEntity<TId>
    {
        IQueryable<T> Entities { get; }

        Task<T?> GetByIdAsync(TId id, CancellationToken cancellationToken);
        Task<List<T>> GetAllAsync(CancellationToken cancellationToken);
        Task<T> AddAsync(T entity, CancellationToken cancellationToken);
        Task UpdateAsync(T entity, CancellationToken cancellationToken);
        Task DeleteAsync(T entity, CancellationToken cancellationToken);
    }
}

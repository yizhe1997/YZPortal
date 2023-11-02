using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IUnitOfWork<TId> : IDisposable
    {
        IGenericRepository<T, TId> Repository<T>() where T : BaseEntity<TId>;

        Task<int> Commit(CancellationToken cancellationToken);

        Task<int> CommitAndRemoveCache(CancellationToken cancellationToken, params string[] cacheKeys);

        Task Rollback();
    }
}

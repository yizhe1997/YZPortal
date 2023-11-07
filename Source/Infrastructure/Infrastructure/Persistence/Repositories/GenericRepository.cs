using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    // TODO: testing
    public class GenericRepository<T, TId> : IGenericRepository<T, TId> where T : class, IEntity<TId>
    {
        private readonly ApplicationDbContext _dbContext;

        public GenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<T> Entities => _dbContext.Set<T>();

        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken)
        {
            await _dbContext.Set<T>().AddAsync(entity, cancellationToken);
            return entity;
        }

        public async Task<List<T>> AddRangeAsync(List<T> entity, CancellationToken cancellationToken)
        {
            await _dbContext.Set<T>().AddRangeAsync(entity, cancellationToken);
            return entity;
        }

        public Task UpdateAsync(T entity, CancellationToken cancellationToken)
        {
            T exist = _dbContext.Set<T>().Find(entity.Id);
            _dbContext.Entry(exist).CurrentValues.SetValues(entity);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(T entity, CancellationToken cancellationToken)
        {
            _dbContext.Set<T>().Remove(entity);
            return Task.CompletedTask;
        }

        public Task DeleteRangeAsync(List<T> entity, CancellationToken cancellationToken)
        {
            _dbContext.Set<T>().RemoveRange(entity);
            return Task.CompletedTask;
        }

        public async Task<List<T>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.Set<T>().ToListAsync(cancellationToken);
        }

        public async Task<T?> GetByIdAsync(TId id, CancellationToken cancellationToken = new CancellationToken())
        {
            return await _dbContext.Set<T>().FindAsync(new object?[] { id }, cancellationToken: cancellationToken);
        }

        public async Task<List<T>> GetByIdsAsync(List<TId> ids, CancellationToken cancellationToken = new CancellationToken())
        {
            return await _dbContext.Set<T>().Where(entity => ids.Contains(entity.Id)).ToListAsync(cancellationToken);
        }
    }
}

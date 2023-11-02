using Domain.Entities.Sync;
using Domain.Entities.Users;
using Domain.Entities.Users.Configs;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces.Contexts
{
    public interface IApplicationDbContext
    {
        #region Data Sets

        #region Users

        public DbSet<Identity> Identities { get; set; }

        #region Configs

        public DbSet<PortalConfig> PortalConfigs { get; set; }

        #endregion

        #endregion

        #region Sync

        public DbSet<SyncStatus> SyncStatuses { get; set; }

        #endregion

        #endregion

        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        public Task BeginTransactionAsync(CancellationToken cancellationToken);

        public Task CommitTransactionAsync(CancellationToken cancellationToken);

        public Task RollbackTransactionAsync(CancellationToken cancellationToken);

    }
}

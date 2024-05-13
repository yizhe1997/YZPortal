using Application.Interfaces.Contexts;
using Application.Interfaces.Services.Identity;
using Domain.Entities.Auditable;
using Domain.Entities.Discounts;
using Domain.Entities.Products;
using Domain.Entities.Sync;
using Domain.Entities.Users;
using Domain.Entities.Users.Configs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Reflection;

namespace Infrastructure.Persistence.Contexts
{
    // TODO: the only reason i havent separate the auditableDbContext from ApplicationDbContext is because almost 
    // eveyrthing will be audited....
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>, IApplicationDbContext
    {
        private IDbContextTransaction? _currentTransaction;
        private readonly ICurrentUserService _currentUserService;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ICurrentUserService currentUserService) : base(options)
        {
            _currentUserService = currentUserService;
        }

        // For unit testing
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        #region Data Sets

        #region Users

        public DbSet<Domain.Entities.Users.Identity> Identities { get; set; }
        public DbSet<UserProfileImage> UserProfileImages { get; set; }

        #region Configs

        public DbSet<PortalConfig> PortalConfigs { get; set; }

        #endregion

        #endregion

        #region Sync

        public DbSet<SyncStatus> SyncStatuses { get; set; }

        #endregion

        #region Discounts

        public DbSet<Discount> Discounts { get; set; }
        public DbSet<DiscountProductMapping> DiscountProductMappings { get; set; }
        public DbSet<DiscountMapping> DiscountMappings { get; set; }
        public DbSet<DiscountManufacturerMapping> DiscountManufacturerMappings { get; set; }
        public DbSet<DiscountProductCategoryMapping> DiscountProductCategoryMappings { get; set; }

        #endregion

        #region Products

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductCategoryPicture> ProductCategoryPictures { get; set; }
        public DbSet<ProductCategoryMapping> ProductCategoryMappings { get; set; }
        public DbSet<ProductAttributeMappingValue> ProductAttributeMappingValues { get; set; }
        public DbSet<ProductAttributeMappingValuePicture> ProductAttributeMappingValuePictures { get; set; }
        public DbSet<ProductAttribute> ProductAttributes { get; set; }
        public DbSet<ProductAttributeMapping> ProductAttributeMappings { get; set; }

        #endregion

        #endregion

        #region DBContext Overrides

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Entity configs
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public override int SaveChanges()
        {
            OnCreateUpdateAuditEntries();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            OnCreateUpdateAuditEntries();
            return base.SaveChangesAsync(cancellationToken);
        }

        #endregion

        #region Auditable Behaviour

        private void OnCreateUpdateAuditEntries()
        {
            var userSubId = _currentUserService.NameIdentifier ?? null;
            var entries = ChangeTracker.Entries<IAuditableEntity>().ToList();

            foreach (var entry in entries) 
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.UtcNow;
                        entry.Entity.CreatedBy = userSubId;
                        break;

                    case EntityState.Modified:
                        entry.Entity.UpdatedDate = DateTime.UtcNow;
                        entry.Entity.UpdatedBy = userSubId;
                        break;
                }
            }
        }

        #endregion

        #region Transaction Behaviour

        public async Task BeginTransactionAsync(CancellationToken cancellationToken)
        {
            if (_currentTransaction != null)
            {
                return;
            }
            else
            {
                _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken);
            }
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken)
        {
            try
            {
                if (_currentTransaction != null)
                {
                    await _currentTransaction.CommitAsync(cancellationToken);
                }
            }
            catch
            {
                await RollbackTransactionAsync(cancellationToken);
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    await _currentTransaction.DisposeAsync();
                    _currentTransaction = null;
                }
            }
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken)
        {
            try
            {
                if (_currentTransaction != null)
                {
                    await _currentTransaction.RollbackAsync(cancellationToken);
                }
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    await _currentTransaction.DisposeAsync();
                    _currentTransaction = null;
                }
            }
        }

        #endregion
    }
}

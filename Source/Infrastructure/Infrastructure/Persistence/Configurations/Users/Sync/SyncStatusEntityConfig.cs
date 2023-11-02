using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.Sync;

namespace Infrastructure.Persistence.Configurations.Users.Sync
{
    public class SyncStatusEntityConfig : IEntityTypeConfiguration<SyncStatus>
    {
        public void Configure(EntityTypeBuilder<SyncStatus> builder)
        {
            builder.HasKey(x => new { x.Type, x.Name });
        }
    }
}

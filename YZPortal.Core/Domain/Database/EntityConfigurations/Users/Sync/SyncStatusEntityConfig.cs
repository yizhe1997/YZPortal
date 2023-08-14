using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using YZPortal.Core.Domain.Database.EntityTypes.Sync;

namespace YZPortal.Core.Domain.Database.EntityConfigurations.Users.Sync
{
    public class SyncStatusEntityConfig : IEntityTypeConfiguration<SyncStatus>
    {
        public void Configure(EntityTypeBuilder<SyncStatus> builder)
        {
            builder.HasKey(x => new { x.Type, x.Name });
        }
    }
}

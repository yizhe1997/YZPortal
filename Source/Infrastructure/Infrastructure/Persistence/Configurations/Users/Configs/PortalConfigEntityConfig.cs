using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.Users.Configs;

namespace Infrastructure.Persistence.Configurations.Users.Configs
{
    public class PortalConfigEntityConfig : IEntityTypeConfiguration<PortalConfig>
    {
        public void Configure(EntityTypeBuilder<PortalConfig> builder)
        {
        }
    }
}

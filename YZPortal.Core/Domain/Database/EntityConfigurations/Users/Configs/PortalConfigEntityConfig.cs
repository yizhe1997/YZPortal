using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using YZPortal.Core.Domain.Database.EntityTypes.Users.Configs;
using YZPortal.Core.Domain.Database.EntityTypes.Users;

namespace YZPortal.Core.Domain.Database.EntityConfigurations.Users.Configs
{
    public class PortalConfigEntityConfig : IEntityTypeConfiguration<PortalConfig>
    {
        public void Configure(EntityTypeBuilder<PortalConfig> builder)
        {
            builder.HasOne(x => x.User)
                .WithOne(x => x.PortalConfig)
                .HasForeignKey<PortalConfig>(x => x.UserSubjectIdentifier)
                .IsRequired()
                .HasPrincipalKey<User>(x => x.SubjectIdentifier)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

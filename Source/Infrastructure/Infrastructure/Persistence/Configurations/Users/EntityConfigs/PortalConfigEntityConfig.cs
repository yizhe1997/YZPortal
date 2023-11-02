using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.Users.Configs;
using Domain.Entities.Users;

namespace Infrastructure.Persistence.Configurations.Users.EntityConfigs
{
    public class PortalConfigEntityConfig : IEntityTypeConfiguration<PortalConfig>
    {
        public void Configure(EntityTypeBuilder<PortalConfig> builder)
        {
            // TODO: move to user instead
            builder.HasOne(x => x.User)
                .WithOne(x => x.PortalConfig)
                .HasForeignKey<PortalConfig>(x => x.UserSubjectIdentifier)
                .IsRequired()
                .HasPrincipalKey<User>(x => x.SubjectIdentifier)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.Users;
using Domain.Entities.Users.Configs;

namespace Infrastructure.Persistence.Configurations.Users
{
    public class UserEntityConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasOne(x => x.PortalConfig)
                .WithOne(x => x.User)
                .HasForeignKey<PortalConfig>(x => x.UserSubjectIdentifier)
                .IsRequired()
                .HasPrincipalKey<User>(x => x.SubjectIdentifier)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Identities)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .IsRequired()
                .HasPrincipalKey(x => x.Id)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

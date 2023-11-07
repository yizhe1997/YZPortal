using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Configurations.Misc
{
    public class FileEntityConfig : IEntityTypeConfiguration<Domain.Entities.Misc.File>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Misc.File> builder)
        {
            // The .WithMany() method is required to complete the relationship configuration,
            // even when the relationship is not a traditional one-to-many or many-to-many relationship.
            builder.HasOne(e => e.User)
            .WithMany()
            .HasForeignKey(e => e.RefId)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

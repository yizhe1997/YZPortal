using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.Discounts;

namespace Infrastructure.Persistence.Configurations.Discounts
{
    public class DiscountProductMappingEntityConfig : IEntityTypeConfiguration<DiscountProductMapping>
    {
        public void Configure(EntityTypeBuilder<DiscountProductMapping> builder)
        {
            builder.HasOne(x => x.Product)
                .WithMany(x => x.DiscountProductMappings)
                .HasForeignKey(x => x.RefId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

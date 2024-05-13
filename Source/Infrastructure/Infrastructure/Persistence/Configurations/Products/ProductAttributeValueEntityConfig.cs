using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.Products;

namespace Infrastructure.Persistence.Configurations.Products
{
    public class ProductAttributeValueEntityConfig : IEntityTypeConfiguration<ProductAttributeMappingValue>
    {
        public void Configure(EntityTypeBuilder<ProductAttributeMappingValue> builder)
        {
            builder.Property(c => c.Cost)
                .HasPrecision(10, 2);
            builder.Property(c => c.PriceAdjustmentUsePercentage)
                .HasPrecision(10, 2);
            builder.Property(c => c.WeightAdjustment)
                .HasPrecision(10, 2);
            builder.Property(c => c.PriceAdjustment)
                .HasPrecision(10, 2);
        }
    }
}

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.Products;

namespace Infrastructure.Persistence.Configurations.Products
{
    public class ProductEntityConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(c => c.Cost)
                .HasPrecision(10, 2);
            builder.Property(c => c.Price)
                .HasPrecision(10, 2);

            builder.HasMany(x => x.DiscountProductMappings)
                .WithOne(x => x.Product)
                .HasForeignKey(x => x.RefId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

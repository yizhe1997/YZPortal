using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.Products;

namespace Infrastructure.Persistence.Configurations.Products
{
    public class ProductAttributeMappingValueEntityConfig : IEntityTypeConfiguration<ProductAttributeMappingValue>
    {
        public void Configure(EntityTypeBuilder<ProductAttributeMappingValue> builder)
        {
            builder.HasOne(x => x.ProductAttributeMappingValuePicture)
               .WithOne()
               .HasForeignKey<ProductAttributeMappingValuePicture>(e => e.RefId);
        }
    }
}

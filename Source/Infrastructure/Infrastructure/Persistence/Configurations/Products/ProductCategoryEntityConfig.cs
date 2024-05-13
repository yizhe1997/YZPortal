using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.Products;

namespace Infrastructure.Persistence.Configurations.Products
{
    public class ProductCategoryEntityConfig : IEntityTypeConfiguration<ProductCategory>
    {
        public void Configure(EntityTypeBuilder<ProductCategory> builder)
        {
            builder.HasOne(x => x.ProductCategoryPicture)
               .WithOne()
               .HasForeignKey<ProductCategoryPicture>(e => e.RefId);

            builder.HasMany(x => x.DiscountProductCategoryMappings)
                .WithOne(x => x.ProductCategory)
                .HasForeignKey(x => x.RefId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

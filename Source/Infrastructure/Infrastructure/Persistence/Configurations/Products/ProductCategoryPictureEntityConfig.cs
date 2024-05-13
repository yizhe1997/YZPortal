using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.Products;

namespace Infrastructure.Persistence.Configurations.Products
{
    public class ProductCategoryPictureEntityConfig : IEntityTypeConfiguration<ProductCategoryPicture>
    {
        public void Configure(EntityTypeBuilder<ProductCategoryPicture> builder)
        {
        }
    }
}

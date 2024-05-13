using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.Discounts;

namespace Infrastructure.Persistence.Configurations.Discounts
{
    public class DiscountProductCategoryMappingEntityConfig : IEntityTypeConfiguration<DiscountProductCategoryMapping>
    {
        public void Configure(EntityTypeBuilder<DiscountProductCategoryMapping> builder)
        {
        }
    }
}

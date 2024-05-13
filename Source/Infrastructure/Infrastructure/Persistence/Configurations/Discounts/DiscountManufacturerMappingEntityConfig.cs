using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.Discounts;

namespace Infrastructure.Persistence.Configurations.Discounts
{
    public class DiscountManufacturerMappingEntityConfig : IEntityTypeConfiguration<DiscountManufacturerMapping>
    {
        public void Configure(EntityTypeBuilder<DiscountManufacturerMapping> builder)
        {
        }
    }
}

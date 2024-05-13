using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.Discounts;

namespace Infrastructure.Persistence.Configurations.Discounts
{
    public class DiscountMappingEntityConfig : IEntityTypeConfiguration<DiscountMapping>
    {
        public void Configure(EntityTypeBuilder<DiscountMapping> builder)
        {
        }
    }
}

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.Discounts;

namespace Infrastructure.Persistence.Configurations.Discounts
{
    public class DiscountEntityConfig : IEntityTypeConfiguration<Discount>
    {
        public void Configure(EntityTypeBuilder<Discount> builder)
        {
            builder.Property(c => c.DiscountAmount)
                .HasPrecision(10, 2);
            builder.Property(c => c.DiscountPercentage)
                .HasPrecision(10, 2);
            builder.Property(c => c.MaxDiscountAmountForPercentage)
                .HasPrecision(10, 2);
        }
    }
}

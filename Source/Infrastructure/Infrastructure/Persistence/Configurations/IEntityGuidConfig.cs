using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class IEntityGuidConfig<T> : IEntityTypeConfiguration<T> where T : class, IEntity<Guid>
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
        }
    }
}

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.Users;

namespace Infrastructure.Persistence.Configurations.Users
{
    public class UserEntityConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
        }
    }
}

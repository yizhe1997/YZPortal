using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using YZPortal.Core.Domain.Database.EntityTypes.Users;

namespace YZPortal.Core.Domain.Database.EntityConfigurations.Users
{
    public class UserEntityConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
        }
    }
}

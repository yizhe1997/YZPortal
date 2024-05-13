using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.Users;

namespace Infrastructure.Persistence.Configurations.Users
{
    public class UserProfileImageEntityConfig : IEntityTypeConfiguration<UserProfileImage>
    {
        public void Configure(EntityTypeBuilder<UserProfileImage> builder)
        {
        }
    }
}

using Application.Interfaces.Repositories;
using Application.Interfaces.Repositories.Users;
using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories.Users
{
    public class UserProfileImageRepository(IGenericRepository<UserProfileImage, Guid> repository) : IUserProfileImageRepository
    {
        public async Task<UserProfileImage?> GetByUserIdFirstOrDefaultAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await repository.Entities.FirstOrDefaultAsync(p => p.RefId == userId, cancellationToken);
        }
    }
}
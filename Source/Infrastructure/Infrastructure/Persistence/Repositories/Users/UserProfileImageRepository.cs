using Application.Interfaces.Repositories;
using Application.Interfaces.Repositories.Users;
using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories.Users
{
    public class UserProfileImageRepository : IUserProfileImageRepository
    {
        private readonly IGenericRepository<UserProfileImage, Guid> _repository;

        public UserProfileImageRepository(IGenericRepository<UserProfileImage, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<UserProfileImage?> GetByUserIdFirstOrDefaultAsync(Guid userId)
        {
            return await _repository.Entities.FirstOrDefaultAsync(p => p.RefId == userId);
        }
    }
}
using Domain.Entities.Users;

namespace Application.Interfaces.Repositories.Users
{
    public interface IUserProfileImageRepository
    {
        Task<UserProfileImage?> GetByUserIdFirstOrDefaultAsync(Guid userId);
    }
}

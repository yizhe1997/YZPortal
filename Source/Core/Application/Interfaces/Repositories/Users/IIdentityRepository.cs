using Domain.Entities.Users;

namespace Application.Interfaces.Repositories.Users
{
    public interface IIdentityRepository
    {
        Task<List<Identity>> GetByUserSubIdAsync(string userSubId, CancellationToken cancellationToken = default);
    }
}

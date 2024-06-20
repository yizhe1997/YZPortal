using Domain.Entities.Users.Configs;

namespace Application.Interfaces.Repositories
{
    public interface IPortalConfigRepository
    {
        Task<PortalConfig?> GetByUserSubIdFirstOrDefaultAsync(string? userSubId, CancellationToken cancellationToken = default);
    }
}

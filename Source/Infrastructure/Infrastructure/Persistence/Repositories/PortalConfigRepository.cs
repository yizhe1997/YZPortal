using Application.Interfaces.Repositories;
using Domain.Entities.Users.Configs;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class PortalConfigRepository(IGenericRepository<PortalConfig, Guid> repository) : IPortalConfigRepository
    {
        public async Task<PortalConfig?> GetByUserSubIdFirstOrDefaultAsync(string? userSubId, CancellationToken cancellationToken = default)
        {
            return await repository.Entities.FirstOrDefaultAsync(p => p.UserSubjectIdentifier == userSubId, cancellationToken: cancellationToken);
        }
    }
}

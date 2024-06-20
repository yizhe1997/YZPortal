using Application.Interfaces.Repositories;
using Domain.Entities.Users.Configs;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class PortalConfigRepository : IPortalConfigRepository
    {
        private readonly IGenericRepository<PortalConfig, Guid> _repository;

        public PortalConfigRepository(IGenericRepository<PortalConfig, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<PortalConfig?> GetByUserSubIdFirstOrDefaultAsync(string? userSubId, CancellationToken cancellationToken = default)
        {
            return await _repository.Entities.FirstOrDefaultAsync(p => p.UserSubjectIdentifier == userSubId, cancellationToken: cancellationToken);
        }
    }
}

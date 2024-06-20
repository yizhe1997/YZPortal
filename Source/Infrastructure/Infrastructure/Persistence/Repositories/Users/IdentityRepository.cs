using Application.Interfaces.Repositories;
using Application.Interfaces.Repositories.Users;
using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Infrastructure.Persistence.Repositories.Users
{
    public class IdentityRepository(IGenericRepository<Identity, Guid> repository) : IIdentityRepository
    {
        public async Task<List<Identity>> GetByUserSubIdAsync(string userSubId, CancellationToken cancellationToken = default)
        {
            return await repository.Entities.Where(p => p.User.SubjectIdentifier == userSubId).ToListAsync(cancellationToken);
        }
    }
}

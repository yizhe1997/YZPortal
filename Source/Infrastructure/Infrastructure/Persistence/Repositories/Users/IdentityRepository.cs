using Application.Interfaces.Repositories;
using Application.Interfaces.Repositories.Users;
using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Infrastructure.Persistence.Repositories.Users
{
    public class IdentityRepository : IIdentityRepository
    {
        private readonly IGenericRepository<Identity, Guid> _repository;

        public IdentityRepository(IGenericRepository<Identity, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<List<Identity>> GetByUserSubIdAsync(string userSubId)
        {
            return await _repository.Entities.Where(p => p.User.SubjectIdentifier == userSubId).ToListAsync();
        }
    }
}

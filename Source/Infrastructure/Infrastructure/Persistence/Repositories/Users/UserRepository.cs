using Application.Interfaces.Repositories;
using Application.Interfaces.Repositories.Users;
using Domain.Entities.Users;

namespace Infrastructure.Persistence.Repositories.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly IGenericRepository<User, Guid> _repository;

        public UserRepository(IGenericRepository<User, Guid> repository)
        {
            _repository = repository;
        }
    }
}

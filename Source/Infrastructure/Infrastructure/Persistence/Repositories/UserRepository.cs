using Application.Interfaces.Repositories;
using Domain.Entities.Users;

namespace Infrastructure.Persistence.Repositories
{
    public  class UserRepository : IUserRepository
    {
        private readonly IGenericRepository<User, Guid> _repository;

        public UserRepository(IGenericRepository<User, Guid> repository)
        {
            _repository = repository;
        }
    }
}

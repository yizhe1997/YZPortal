using Application.Interfaces.Repositories;
using Application.Interfaces.Repositories.Users;
using Domain.Entities.Users;

namespace Infrastructure.Persistence.Repositories.Users
{
    public class UserRepository(IGenericRepository<User, Guid> repository) : IUserRepository
    {
    }
}

using Application.Interfaces.Repositories;
using Application.Interfaces.Repositories.Users;
using Domain.Entities.Discounts;

namespace Infrastructure.Persistence.Repositories.Discounts
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IGenericRepository<Discount, Guid> _repository;

        public DiscountRepository(IGenericRepository<Discount, Guid> repository)
        {
            _repository = repository;
        }
    }
}

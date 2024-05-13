using Application.Interfaces.Repositories;
using Application.Interfaces.Repositories.Users;
using Domain.Entities.Discounts;

namespace Infrastructure.Persistence.Repositories.Discounts
{
    public class DiscountProductMappingRepository : IDiscountProductMappingRepository
    {
        private readonly IGenericRepository<DiscountProductMapping, Guid> _repository;

        public DiscountProductMappingRepository(IGenericRepository<DiscountProductMapping, Guid> repository)
        {
            _repository = repository;
        }
    }
}

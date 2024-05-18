using Application.Interfaces.Repositories;
using Application.Interfaces.Repositories.Discounts;
using Domain.Entities.Discounts;

namespace Infrastructure.Persistence.Repositories.Discounts
{
    public class DiscountMappingRepository : IDiscountMappingRepository
    {
        private readonly IGenericRepository<DiscountMapping, Guid> _repository;

        public DiscountMappingRepository(IGenericRepository<DiscountMapping, Guid> repository)
        {
            _repository = repository;
        }
    }
}

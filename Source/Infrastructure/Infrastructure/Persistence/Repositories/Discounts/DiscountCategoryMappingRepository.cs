using Application.Interfaces.Repositories;
using Application.Interfaces.Repositories.Discounts;
using Domain.Entities.Discounts;

namespace Infrastructure.Persistence.Repositories.Discounts
{
    public class DiscountCategoryMappingRepository : IDiscountCategoryMappingRepository
    {
        private readonly IGenericRepository<DiscountProductCategoryMapping, Guid> _repository;

        public DiscountCategoryMappingRepository(IGenericRepository<DiscountProductCategoryMapping, Guid> repository)
        {
            _repository = repository;
        }
    }
}

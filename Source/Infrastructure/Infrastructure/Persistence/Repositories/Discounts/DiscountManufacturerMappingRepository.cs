using Application.Interfaces.Repositories;
using Application.Interfaces.Repositories.Users;
using Domain.Entities.Discounts;

namespace Infrastructure.Persistence.Repositories.Discounts
{
    public class DiscountManufacturerMappingRepository : IDiscountManufacturerMappingRepository
    {
        private readonly IGenericRepository<DiscountManufacturerMapping, Guid> _repository;

        public DiscountManufacturerMappingRepository(IGenericRepository<DiscountManufacturerMapping, Guid> repository)
        {
            _repository = repository;
        }
    }
}

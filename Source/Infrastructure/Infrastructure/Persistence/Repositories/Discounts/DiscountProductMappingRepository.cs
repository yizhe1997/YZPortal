using Application.Interfaces.Repositories;
using Application.Interfaces.Repositories.Discounts;
using Domain.Entities.Discounts;

namespace Infrastructure.Persistence.Repositories.Discounts
{
    public class DiscountProductMappingRepository(IGenericRepository<DiscountProductMapping, Guid> repository) : IDiscountProductMappingRepository
    {
    }
}

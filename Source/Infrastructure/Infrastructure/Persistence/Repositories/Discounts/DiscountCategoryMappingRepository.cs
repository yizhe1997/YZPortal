using Application.Interfaces.Repositories;
using Application.Interfaces.Repositories.Discounts;
using Domain.Entities.Discounts;

namespace Infrastructure.Persistence.Repositories.Discounts
{
    public class DiscountCategoryMappingRepository(IGenericRepository<DiscountProductCategoryMapping, Guid> repository) : IDiscountCategoryMappingRepository
    {
    }
}

using Application.Interfaces.Repositories;
using Application.Interfaces.Repositories.Discounts;
using Domain.Entities.Discounts;

namespace Infrastructure.Persistence.Repositories.Discounts
{
    public class DiscountRepository(IGenericRepository<Discount, Guid> repository) : IDiscountRepository
    {
    }
}

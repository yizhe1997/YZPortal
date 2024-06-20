using Application.Interfaces.Repositories;
using Application.Interfaces.Repositories.Products;
using Domain.Entities.Products;

namespace Infrastructure.Persistence.Repositories.Products
{
    public class ProductCategoryRepository(IGenericRepository<ProductCategory, Guid> repository) : IProductCategoryRepository
    {
    }
}

using Application.Interfaces.Repositories;
using Application.Interfaces.Repositories.Products;
using Domain.Entities.Products;

namespace Infrastructure.Persistence.Repositories.Products
{
    public class ProductRepository(IGenericRepository<Product, Guid> repository) : IProductRepository
    {
    }
}

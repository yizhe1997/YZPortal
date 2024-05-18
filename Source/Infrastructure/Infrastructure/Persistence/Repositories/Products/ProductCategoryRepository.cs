using Application.Interfaces.Repositories;
using Application.Interfaces.Repositories.Products;
using Domain.Entities.Products;

namespace Infrastructure.Persistence.Repositories.Products
{
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly IGenericRepository<ProductCategory, Guid> _repository;

        public ProductCategoryRepository(IGenericRepository<ProductCategory, Guid> repository)
        {
            _repository = repository;
        }
    }
}

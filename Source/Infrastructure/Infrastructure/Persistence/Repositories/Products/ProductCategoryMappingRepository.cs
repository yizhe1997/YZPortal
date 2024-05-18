using Application.Interfaces.Repositories;
using Application.Interfaces.Repositories.Products;
using Domain.Entities.Products;

namespace Infrastructure.Persistence.Repositories.Products
{
    public class ProductCategoryMappingRepository : IProductCategoryMappingRepository
    {
        private readonly IGenericRepository<ProductCategoryMapping, Guid> _repository;

        public ProductCategoryMappingRepository(IGenericRepository<ProductCategoryMapping, Guid> repository)
        {
            _repository = repository;
        }
    }
}

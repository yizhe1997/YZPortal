using Application.Interfaces.Repositories;
using Application.Interfaces.Repositories.Products;
using Domain.Entities.Products;

namespace Infrastructure.Persistence.Repositories.Products
{
    public class ProductAttributeMappingRepository : IProductAttributeMappingRepository
    {
        private readonly IGenericRepository<ProductAttributeMapping, Guid> _repository;

        public ProductAttributeMappingRepository(IGenericRepository<ProductAttributeMapping, Guid> repository)
        {
            _repository = repository;
        }
    }
}

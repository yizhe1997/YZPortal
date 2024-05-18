using Application.Interfaces.Repositories;
using Application.Interfaces.Repositories.Products;
using Domain.Entities.Products;

namespace Infrastructure.Persistence.Repositories.Products
{
    public class ProductAttributeValueRepository : IProductAttributeValueRepository
    {
        private readonly IGenericRepository<ProductAttributeMappingValue, Guid> _repository;

        public ProductAttributeValueRepository(IGenericRepository<ProductAttributeMappingValue, Guid> repository)
        {
            _repository = repository;
        }
    }
}

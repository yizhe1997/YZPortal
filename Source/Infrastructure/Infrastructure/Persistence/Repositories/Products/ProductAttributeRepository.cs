using Application.Interfaces.Repositories;
using Application.Interfaces.Repositories.Products;
using Domain.Entities.Products;

namespace Infrastructure.Persistence.Repositories.Products
{
    public class ProductAttributeRepository : IProductAttributeRepository
    {
        private readonly IGenericRepository<ProductAttribute, Guid> _repository;

        public ProductAttributeRepository(IGenericRepository<ProductAttribute, Guid> repository)
        {
            _repository = repository;
        }
    }
}

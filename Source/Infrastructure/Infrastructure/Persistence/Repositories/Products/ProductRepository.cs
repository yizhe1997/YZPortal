using Application.Interfaces.Repositories;
using Application.Interfaces.Repositories.Users;
using Domain.Entities.Products;

namespace Infrastructure.Persistence.Repositories.Products
{
    public class ProductRepository : IProductRepository
    {
        private readonly IGenericRepository<Product, Guid> _repository;

        public ProductRepository(IGenericRepository<Product, Guid> repository)
        {
            _repository = repository;
        }
    }
}

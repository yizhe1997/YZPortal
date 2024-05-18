using Application.Interfaces.Repositories;
using Application.Interfaces.Repositories.Products;
using Domain.Entities.Products;

namespace Infrastructure.Persistence.Repositories.Products
{
    public class ProductCategoryPictureRepository : IProductRepository
    {
        private readonly IGenericRepository<ProductCategoryPicture, Guid> _repository;

        public ProductCategoryPictureRepository(IGenericRepository<ProductCategoryPicture, Guid> repository)
        {
            _repository = repository;
        }
    }
}

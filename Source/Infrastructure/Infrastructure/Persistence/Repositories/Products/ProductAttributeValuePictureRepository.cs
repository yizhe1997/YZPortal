using Application.Interfaces.Repositories;
using Application.Interfaces.Repositories.Products;
using Domain.Entities.Products;

namespace Infrastructure.Persistence.Repositories.Products
{
    public class ProductAttributeValuePictureRepository : IProductAttributeValuePicture
    {
        private readonly IGenericRepository<ProductAttributeMappingValuePicture, Guid> _repository;

        public ProductAttributeValuePictureRepository(IGenericRepository<ProductAttributeMappingValuePicture, Guid> repository)
        {
            _repository = repository;
        }
    }
}

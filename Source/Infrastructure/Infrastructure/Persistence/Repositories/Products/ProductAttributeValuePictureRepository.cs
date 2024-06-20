using Application.Interfaces.Repositories;
using Application.Interfaces.Repositories.Products;
using Domain.Entities.Products;

namespace Infrastructure.Persistence.Repositories.Products
{
    public class ProductAttributeValuePictureRepository(IGenericRepository<ProductAttributeMappingValuePicture, Guid> repository) : IProductAttributeValuePicture
    {
    }
}

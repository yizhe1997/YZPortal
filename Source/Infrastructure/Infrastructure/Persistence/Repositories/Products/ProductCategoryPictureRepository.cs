using Application.Interfaces.Repositories;
using Application.Interfaces.Repositories.Products;
using Domain.Entities.Products;

namespace Infrastructure.Persistence.Repositories.Products
{
    public class ProductCategoryPictureRepository(IGenericRepository<ProductCategoryPicture, Guid> repository) : IProductRepository
    {
    }
}

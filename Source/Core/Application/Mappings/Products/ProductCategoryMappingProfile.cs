using Application.Features.Products.Queries.GetProductCategories;
using AutoMapper;
using Domain.Entities.Products;

namespace Application.Mappings.Products
{
    public class ProductCategoryMappingProfile : Profile
    {
        public ProductCategoryMappingProfile()
        {
            CreateMap<ProductCategory, ProductCategoryDto>();
        }
    }
}
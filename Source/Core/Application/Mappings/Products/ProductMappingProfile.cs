using Application.Features.Products.Queries.GetProducts;
using AutoMapper;
using Domain.Entities.Products;

namespace Application.Mappings.Products
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<Product, ProductDto>();
        }
    }
}
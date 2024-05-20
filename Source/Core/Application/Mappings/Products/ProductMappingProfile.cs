using Application.Features.Products.Commands.AddProduct;
using Application.Features.Products.Queries.GetProduct;
using Application.Features.Products.Queries.GetProducts;
using Application.Features.Products.Queries.GetProductsExport;
using AutoMapper;
using Domain.Entities.Products;

namespace Application.Mappings.Products
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<Product, GetProductByIdDto>();
            CreateMap<AddProductCommand, Product>();
            CreateMap<Product, ProductExportDto>();
        }
    }
}
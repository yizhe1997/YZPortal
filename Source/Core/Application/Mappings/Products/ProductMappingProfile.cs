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
            CreateMap<AddProductCommand, Product>()
                .ForMember(x => x.IsPublished, opt => opt.Ignore())
                .ForMember(x => x.StockQuantity, opt => opt.Ignore())
                .ForMember(x => x.Price, opt => opt.Ignore())
                .ForMember(x => x.Cost, opt => opt.Ignore())
                .ForMember(x => x.DiscountProductMappings, opt => opt.Ignore())
                .ForMember(x => x.ProductCategoryMappings, opt => opt.Ignore())
                .ForMember(x => x.ProductAttributeMappings, opt => opt.Ignore())
                .ForMember(x => x.CreatedDate, opt => opt.Ignore())
                .ForMember(x => x.CreatedBy, opt => opt.Ignore())
                .ForMember(x => x.UpdatedDate, opt => opt.Ignore())
                .ForMember(x => x.UpdatedBy, opt => opt.Ignore())
                .ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<Product, ProductExportDto>();
        }
    }
}
using Application.Features.Products.Commands.AddProduct;
using Application.Features.Products.Queries.GetProduct;
using Application.Features.Products.Queries.GetProducts;
using Application.Features.Products.Queries.GetProductsExport;
using Application.Mappings.Products;
using Domain.Entities.Products;

namespace Application.UnitTests.Mappings.Users.Configs
{
    public class ProductMappingTests : MappingsTestsBase<ProductMappingProfile>
    {
        public ProductMappingTests()
        {
        }

        [Fact]
        public void ShouldHaveValidConfiguration()
        {
            // Arrange

            // Act

            // Assert
            _mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }

        [Theory]
        [InlineData(typeof(Product), typeof(ProductDto))]
        [InlineData(typeof(Product), typeof(GetProductByIdDto))]
        [InlineData(typeof(AddProductCommand), typeof(Product))]
        [InlineData(typeof(Product), typeof(ProductExportDto))]
        public void ShouldSupportMappingFromSourceToDestination(Type sourceType, Type destinationType)
        {
            // arrange
            var instance = Application.Extensions.TypeExtensions.GetInstanceOf(sourceType);

            // act


            // assert
            _mapper.Map(instance, sourceType, destinationType);
        }
    }
}

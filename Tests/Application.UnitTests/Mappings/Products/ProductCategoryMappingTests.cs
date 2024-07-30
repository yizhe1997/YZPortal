using Application.Features.Products.Queries.GetProductCategories;
using Application.Features.Products.Queries.GetProductCategoriesExport;
using Application.Mappings.Products;
using Domain.Entities.Products;

namespace Application.UnitTests.Mappings.Users.Configs
{
    public class ProductCategoryMappingTests : MappingsTestsBase<ProductCategoryMappingProfile>
    {
        public ProductCategoryMappingTests()
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
        [InlineData(typeof(ProductCategory), typeof(ProductCategoryDto))]
        [InlineData(typeof(ProductCategory), typeof(ProductCategoryExportDto))]
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

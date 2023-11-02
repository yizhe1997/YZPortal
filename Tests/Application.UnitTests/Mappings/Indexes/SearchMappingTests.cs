using Application.Mappings.Indexes;
using Application.Models;
using Application.Models.Identity;
using Domain.Entities.Users;

namespace Application.UnitTests.Mappings.Indexes
{
    public class SearchMappingTests : MappingsTestsBase<SearchMappingProfile>
    {
        public SearchMappingTests()
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
        [InlineData(typeof(SearchResult<User>), typeof(SearchResult<UserModel>))]
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

using Application.Features.Users.Configs.Commands.UpdatePortalConfig;
using Application.Features.Users.Configs.Queries.GetConfigs;
using Application.Mappings.Users.Configs;
using Domain.Entities.Users.Configs;

namespace Application.UnitTests.Mappings.Users.Configs
{
    public class PortalConfigMappingTests : MappingsTestsBase<PortalConfigMappingProfile>
    {
        public PortalConfigMappingTests()
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
        [InlineData(typeof(UpdateUserPortalConfigCommand), typeof(PortalConfig))]
        [InlineData(typeof(PortalConfig), typeof(PortalConfigDto))]
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

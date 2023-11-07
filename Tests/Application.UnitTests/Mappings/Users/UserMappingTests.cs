using Application.Interfaces.Services.Identity;
using Application.Mappings.Users;
using Application.Models.Identity;
using Application.Requests.Users;
using Domain.Entities.Users;

namespace Application.UnitTests.Mappings.Users
{
    public class UserMappingTests : MappingsTestsBase<UserMappingProfile>
    {
        public UserMappingTests()
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
        //[InlineData(typeof(ICurrentUserService), typeof(User))] // TODO: look into this
        [InlineData(typeof(Identity), typeof(IdentityModel))]
        [InlineData(typeof(User), typeof(UserModel))]
        [InlineData(typeof(UpdateUserCommand), typeof(User))]
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

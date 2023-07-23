using YZPortal.Core.Domain.Database;


namespace YZPortal.UnitTest
{
    public class UnitTestDatabaseService
    {
        private readonly Mock<IDatabaseService> databaseService;
        public UnitTestDatabaseService()
        {
            databaseService = new Mock<IDatabaseService>();
        }

        //[Fact]
        //public void Test1()
        //{

        //}

        //[Theory]
        //[InlineAutoData("Prasad")]
        //public async Task UserGetBySubIdAsync(string subId, User user)
        //{
        //    // Arrange
        //    sut.FirstName = firstName;
        //    sut.LastName = lastName;
        //    sut.MiddleName = middleName;
        //    var actual = sut.FullName;

        //    // Act
        //    await databaseService.Object.UserGetBySubIdAsync(subId);

        //    // Assert
        //    Assert.Equal(expected, actual);
        //}
    }
}
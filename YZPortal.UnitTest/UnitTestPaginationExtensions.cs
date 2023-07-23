using System.Diagnostics;
using Xunit.Abstractions;
using YZPortal.API.Controllers.ControllerRequests.Indexes;
using YZPortal.Core.Domain.Contexts;
using YZPortal.Core.Domain.Database.Users;
using YZPortal.Core.Indexes;
using YZPortal.UnitTest.Domain.Contexts;

namespace YZPortal.UnitTest
{
    public class UnitTestPaginationExtensions
    {
        private readonly Fixture _fixture = new();
        private readonly PortalContext _portalContext = Context.CreatePortalContext();
        private readonly int _userCount = 10000;
        private readonly ITestOutputHelper _output;

        public UnitTestPaginationExtensions(ITestOutputHelper output)
        {
            PopulateDbContext(_portalContext);
            _output = output;
        }

        private void PopulateDbContext(PortalContext portalContext)
        {
            var users = _fixture.CreateMany<User>(_userCount);
            portalContext.Users.AddRange(users);

            portalContext.SaveChanges();
        }

        public enum PageType
        {
            FirstPage,
            MiddlePage,
            LastPage
        }

        public int GetPageNumber(PageType pageType, int totalPages)
        {
            switch (pageType)
            {
                case PageType.FirstPage:
                    return 1;
                case PageType.MiddlePage:
                    return totalPages / 2;
                case PageType.LastPage:
                    return totalPages;
                default:
                    return 1; // Default to the first page
            }
        }

        public enum MaxPageType
        {
            LessThanDefault,
            Default,
            MiddlePage,
            GreaterThanTotalPage
        }

        public int GetMaxPageNumber(MaxPageType maxPageType, int totalPages)
        {
            switch (maxPageType)
            {
                case MaxPageType.LessThanDefault:
                    return -2;
                case MaxPageType.Default:
                    return -1;
                case MaxPageType.MiddlePage:
                    return totalPages / 2;
                case MaxPageType.GreaterThanTotalPage:
                    return totalPages + 1;
                default:
                    return 1; // Default to the first page
            }
        }

        [Theory]
        [InlineData(10, PageType.FirstPage)] // Edge case: First page
        [InlineData(10, PageType.MiddlePage)] // Middle page
        [InlineData(10, PageType.LastPage)] // Edge case: Last page
        [InlineData(10, PageType.MiddlePage, MaxPageType.MiddlePage)] // Test with maxPages set to middle page
        [InlineData(10, PageType.MiddlePage, MaxPageType.GreaterThanTotalPage)] // Edge case: Test with maxPages set to larger than total pages
        [InlineData(10, PageType.MiddlePage, MaxPageType.LessThanDefault)] // Edge case: Test with maxPages set to less than 0
        [InlineData(10, PageType.MiddlePage, MaxPageType.Default, true)] // Test with empty data source
        public void CreateShouldReturnCorrectPaginatedList(int pageSize, PageType pageType, MaxPageType maxPageType = MaxPageType.Default, bool isUserListEmpty = false)
        {
            #region Arrange
            var userCount = isUserListEmpty ? 0 : _userCount;
            var totalPages = (int)Math.Ceiling(userCount / (float)pageSize);
            var pageNumber = GetPageNumber(pageType, totalPages);
            var maxPages = GetMaxPageNumber(maxPageType, totalPages);
            var paginationParams = new PagedRequest<User> { PageSize = pageSize, PageNumber = pageNumber };

            #endregion

            #region Act

            // Start a stopwatch to measure performance
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var paginatedList = isUserListEmpty ? new PaginatedList<User>() { PageSize = pageSize, PageNumber = pageNumber } : _portalContext.Users.Create(paginationParams, maxPages);
            
            // Stop the stopwatch
            stopwatch.Stop();

            // Print the elapsed time to the console
            _output.WriteLine($"Elapsed Time (Sync): {stopwatch.ElapsedMilliseconds} ms");

            #endregion

            #region Assert

            Assert.NotNull(paginatedList);
            Assert.Equal(pageSize, paginatedList.PageSize);
            Assert.Equal(pageNumber, paginatedList.PageNumber);
            Assert.Equal(userCount, paginatedList.TotalItems);
            Assert.Equal(totalPages, paginatedList.TotalPages);

            // Check if the pagination is completed within an acceptable time frame
            Assert.True(stopwatch.ElapsedMilliseconds < 1000, "Pagination took too long."); // Adjust the time threshold as per your requirements
            
            #endregion
        }

        [Theory]
        [InlineData(10, PageType.FirstPage)] // Edge case: First page
        [InlineData(10, PageType.MiddlePage)] // Middle page
        [InlineData(10, PageType.LastPage)] // Edge case: Last page
        [InlineData(10, PageType.MiddlePage, MaxPageType.MiddlePage)] // Test with maxPages set to middle page
        [InlineData(10, PageType.MiddlePage, MaxPageType.GreaterThanTotalPage)] // Edge case: Test with maxPages set to larger than total pages
        [InlineData(10, PageType.MiddlePage, MaxPageType.LessThanDefault)] // Edge case: Test with maxPages set to less than 0
        [InlineData(10, PageType.MiddlePage, MaxPageType.Default, true)] // Test with empty data source
        public async Task CreateAsyncShouldReturnCorrectPaginatedList(int pageSize, PageType pageType, MaxPageType maxPageType = MaxPageType.Default, bool isUserListEmpty = false)
        {
            #region Arrange
            var userCount = isUserListEmpty ? 0 : _userCount;
            var totalPages = (int)Math.Ceiling(userCount / (float)pageSize);
            var pageNumber = GetPageNumber(pageType, totalPages);
            var maxPages = GetMaxPageNumber(maxPageType, totalPages);
            var paginationParams = new PagedRequest<User> { PageSize = pageSize, PageNumber = pageNumber };

            #endregion

            #region Act

            // Start a stopwatch to measure performance
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var paginatedList = isUserListEmpty ? new PaginatedList<User>() { PageSize = pageSize, PageNumber = pageNumber } : await _portalContext.Users.CreateAsync(paginationParams, maxPages);

            // Stop the stopwatch
            stopwatch.Stop();

            // Print the elapsed time to the console
            _output.WriteLine($"Elapsed Time (Sync): {stopwatch.ElapsedMilliseconds} ms");

            #endregion

            #region Assert

            Assert.NotNull(paginatedList);
            Assert.Equal(pageSize, paginatedList.PageSize);
            Assert.Equal(pageNumber, paginatedList.PageNumber);
            Assert.Equal(userCount, paginatedList.TotalItems);
            Assert.Equal(totalPages, paginatedList.TotalPages);

            // Check if the pagination is completed within an acceptable time frame
            Assert.True(stopwatch.ElapsedMilliseconds < 1000, "Pagination took too long."); // Adjust the time threshold as per your requirements

            #endregion
        }
    }
}

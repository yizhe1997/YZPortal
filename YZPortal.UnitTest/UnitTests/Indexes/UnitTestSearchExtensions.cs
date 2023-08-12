using System.Diagnostics;
using Xunit.Abstractions;
using static YZPortal.UnitTest.UnitTests.Indexes.Enums.PageType;
using YZPortal.Core.Indexes;
using YZPortal.Core.Domain.Database.Users;
using YZPortal.UnitTest.Domain.Contexts;
using YZPortal.Core.Domain.Database;
using YZPortal.FullStackCore.Requests.Indexes;

namespace YZPortal.UnitTest.UnitTests.Indexes
{
    public class UnitTestSearchExtensions : UnitTestBaseTemplate
    {
        public UnitTestSearchExtensions(ITestOutputHelper output) : base(output)
        {
        }

        // TODO: create unit test for searchstring, orderby, select
        [Theory]
        [InlineData(10, PageTypes.FirstPage)] // Edge case: First page
        [InlineData(10, PageTypes.MiddlePage)] // Middle page
        [InlineData(10, PageTypes.LastPage)] // Edge case: Last page
        [InlineData(10, PageTypes.MiddlePage, true)] // Test with empty data source with anything but first page
        [InlineData(10, PageTypes.FirstPage, true)] // Test with empty data source with first page
        public void CreateSearchListFromIQueryableShouldReturnCorrectSearchList(int pageSize, PageTypes pageType, bool isUserListEmpty = false)
        {
            #region Arrange

            var userCount = _userCount;
            if (isUserListEmpty)
            {
                PortalContextHelpers.UsersDeleteAllFromPortalContext(_portalContext);
                userCount = _portalContext.UsersGetCount();
            }

            var totalPages = (int)Math.Ceiling(userCount / (float)pageSize);
            totalPages = totalPages == 0 ? 1 : totalPages;

            var pageNumber = GetPageNumber(pageType, totalPages);
            var paginationParams = new SearchRequest { PageSize = pageSize, PageNumber = pageNumber };

            #endregion

            #region Act

            // Start a stopwatch to measure performance
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var paginatedList = _portalContext.Users.CreateSearchList(paginationParams);

            // Stop the stopwatch
            stopwatch.Stop();

            // Print the elapsed time to the console
            _output.WriteLine($"Elapsed Time (Sync): {stopwatch.ElapsedMilliseconds} ms");

            #endregion

            #region Assert

            Assert.IsType<SearchList<User>>(paginatedList);
            Assert.NotNull(paginatedList);
            Assert.Equal(pageSize, paginatedList.PageSize);
            Assert.Equal(isUserListEmpty ? 1 : pageNumber, paginatedList.PageNumber);
            Assert.Equal(userCount, paginatedList.TotalItems);
            Assert.Equal(totalPages, paginatedList.TotalPages);

            // Check if the pagination is completed within an acceptable time frame
            Assert.True(stopwatch.ElapsedMilliseconds < _expectedMaxElapsedMilliseconds, "Pagination took too long."); // Adjust the time threshold as per your requirements

            #endregion
        }

        // TODO: create unit test for searchstring, orderby, select
        [Theory]
        [InlineData(10, PageTypes.FirstPage)] // Edge case: First page
        [InlineData(10, PageTypes.MiddlePage)] // Middle page
        [InlineData(10, PageTypes.LastPage)] // Edge case: Last page
        [InlineData(10, PageTypes.MiddlePage, true)] // Test with empty data source with anything but first page
        [InlineData(10, PageTypes.FirstPage, true)] // Test with empty data source with first page
        public async Task CreateSearchListAsyncFromIQueryableShouldReturnCorrectSearchList(int pageSize, PageTypes pageType, bool isUserListEmpty = false)
        {
            #region Arrange

            var userCount = _userCount;
            if (isUserListEmpty)
            {
                PortalContextHelpers.UsersDeleteAllFromPortalContext(_portalContext);
                userCount = _portalContext.UsersGetCount();
            }

            var totalPages = (int)Math.Ceiling(userCount / (float)pageSize);
            totalPages = totalPages == 0 ? 1 : totalPages;

            var pageNumber = GetPageNumber(pageType, totalPages);
            var paginationParams = new SearchRequest { PageSize = pageSize, PageNumber = pageNumber };

            #endregion

            #region Act

            // Start a stopwatch to measure performance
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var paginatedList = await _portalContext.Users.CreateSearchListAsync(paginationParams);

            // Stop the stopwatch
            stopwatch.Stop();

            // Print the elapsed time to the console
            _output.WriteLine($"Elapsed Time (Sync): {stopwatch.ElapsedMilliseconds} ms");

            #endregion

            #region Assert

            Assert.IsType<SearchList<User>>(paginatedList);
            Assert.NotNull(paginatedList);
            Assert.Equal(pageSize, paginatedList.PageSize);
            Assert.Equal(isUserListEmpty ? 1 : pageNumber, paginatedList.PageNumber);
            Assert.Equal(userCount, paginatedList.TotalItems);
            Assert.Equal(totalPages, paginatedList.TotalPages);

            // Check if the pagination is completed within an acceptable time frame
            Assert.True(stopwatch.ElapsedMilliseconds < _expectedMaxElapsedMilliseconds, "Pagination took too long."); // Adjust the time threshold as per your requirements

            #endregion
        }

        // TODO: create unit test for searchstring, orderby, select
        [Theory]
        [InlineData(10, PageTypes.FirstPage)] // Edge case: First page
        [InlineData(10, PageTypes.MiddlePage)] // Middle page
        [InlineData(10, PageTypes.LastPage)] // Edge case: Last page
        [InlineData(10, PageTypes.MiddlePage, true)] // Test with empty data source with anything but first page
        [InlineData(10, PageTypes.FirstPage, true)] // Test with empty data source with first page
        public void CreateSearchListFromListShouldReturnCorrectSearchList(int pageSize, PageTypes pageType, bool isUserListEmpty = false)
        {
            #region Arrange

            var userCount = _userCount;
            if (isUserListEmpty)
            {
                PortalContextHelpers.UsersDeleteAllFromPortalContext(_portalContext);
                userCount = _portalContext.UsersGetCount();
            }

            var totalPages = (int)Math.Ceiling(userCount / (float)pageSize);
            totalPages = totalPages == 0 ? 1 : totalPages;

            var pageNumber = GetPageNumber(pageType, totalPages);
            var paginationParams = new SearchRequest { PageSize = pageSize, PageNumber = pageNumber };

            #endregion

            #region Act

            // Start a stopwatch to measure performance
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var paginatedList = _portalContext.Users.ToList().CreateSearchList(paginationParams);

            // Stop the stopwatch
            stopwatch.Stop();

            // Print the elapsed time to the console
            _output.WriteLine($"Elapsed Time (Sync): {stopwatch.ElapsedMilliseconds} ms");

            #endregion

            #region Assert

            Assert.IsType<SearchList<User>>(paginatedList);
            Assert.NotNull(paginatedList);
            Assert.Equal(pageSize, paginatedList.PageSize);
            Assert.Equal(isUserListEmpty ? 1 : pageNumber, paginatedList.PageNumber);
            Assert.Equal(userCount, paginatedList.TotalItems);
            Assert.Equal(totalPages, paginatedList.TotalPages);

            // Check if the pagination is completed within an acceptable time frame
            Assert.True(stopwatch.ElapsedMilliseconds < _expectedMaxElapsedMilliseconds, "Pagination took too long."); // Adjust the time threshold as per your requirements

            #endregion
        }
    }
}

namespace YZPortal.UnitTest.UnitTests.Indexes.Enums
{
    public class PageType
    {
        public enum PageTypes
        {
            FirstPage,
            MiddlePage,
            LastPage
        }

        public static int GetPageNumber(PageTypes pageType, int totalPages)
        {
            if (totalPages < 1)
                return 1;

            return pageType switch
            {
                PageTypes.FirstPage => 1,
                PageTypes.MiddlePage => (int)Math.Ceiling(totalPages / 2.0),
                PageTypes.LastPage => totalPages,
                _ => 1,// Default to the first page
            };
        }
    }
}

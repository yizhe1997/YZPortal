namespace YZPortal.FullStackCore.Requests.Indexes
{
    public interface ISearchParams : IPaginationParams
    {
        public string SearchString { get; set; }
        public string Lang { get; set; }
        public string[] Select { get; set; }
        public string[] OrderBy { get; set; }
    }
}

namespace YZPortal.FullStackCore.Requests.Indexes
{
    public interface IPaginationParams
    {
        int PageNumber { get; set; }
        int PageSize { get; set; }
    }
}

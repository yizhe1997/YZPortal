namespace YZPortal.Core.Indexes
{
    public interface IPaginationParams
    {
        int PageNumber { get; set; }
        int PageSize { get; set; }
    }
}

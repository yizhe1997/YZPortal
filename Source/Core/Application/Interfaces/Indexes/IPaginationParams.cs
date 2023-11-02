namespace Application.Interfaces.Indexes
{
    public interface IPaginationParams
    {
        int PageNumber { get; set; }
        int PageSize { get; set; }
        int MaxPages { get; set; }
    }
}

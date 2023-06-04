using MediatR;

namespace YZPortal.API.Controllers.Pagination
{
    public class PagedRequest<T> : IPaginationParams, IRequest<T>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}

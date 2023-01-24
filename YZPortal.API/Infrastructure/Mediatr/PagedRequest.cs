using MediatR;
using YZPortal.API.Controllers.Pagination;

namespace YZPortal.API.Infrastructure.Mediatr
{
    public class PagedRequest<T> : IPaginationParams, IRequest<T>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 50;
    }
}

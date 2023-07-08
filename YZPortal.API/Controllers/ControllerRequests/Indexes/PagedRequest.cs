using MediatR;
using YZPortal.Core.Indexes;

namespace YZPortal.API.Controllers.ControllerRequests.Indexes
{
    public class PagedRequest<T> : IPaginationParams, IRequest<T>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}

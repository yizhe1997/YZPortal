using AutoMapper;
using MediatR;
using YZPortal.Core.Domain.Contexts;
using YZPortal.Core.Domain.Database;
using YZPortal.FullStackCore.Requests.Indexes;

namespace YZPortal.API.Infrastructure.Mediatr
{
    public abstract class PaginationRequestHandler<TRequest, TResponse> : BaseRequestHandler<TRequest, TResponse> where TRequest : PagedRequest, IRequest<TResponse>
    {
        public PaginationRequestHandler(DatabaseService dbService, IMapper mapper, IHttpContextAccessor httpContext, CurrentContext userAccessor) : base(dbService, mapper, httpContext, userAccessor)
        {
        }
    }
}

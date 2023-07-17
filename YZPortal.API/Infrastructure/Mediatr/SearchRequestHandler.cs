using AutoMapper;
using YZPortal.API.Controllers.ControllerRequests.Indexes;
using YZPortal.Core.Domain.Contexts;
using YZPortal.Core.Domain.Database;

namespace YZPortal.API.Infrastructure.Mediatr
{
    public abstract class SearchRequestHandler<TRequest, TResponse> : BaseRequestHandler<TRequest, TResponse> where TRequest : SearchRequest<TResponse>
    {
        public SearchRequestHandler(DatabaseService dbService, IMapper mapper, IHttpContextAccessor httpContext, CurrentContext userAccessor) : base(dbService, mapper, httpContext, userAccessor)
        {
        }
    }
}

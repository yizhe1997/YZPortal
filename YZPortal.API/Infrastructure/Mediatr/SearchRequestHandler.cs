using AutoMapper;
using MediatR;
using YZPortal.Core.Domain.Contexts;
using YZPortal.Core.Domain.Database;
using YZPortal.FullStackCore.Requests.Indexes;

namespace YZPortal.API.Infrastructure.Mediatr
{
    public abstract class SearchRequestHandler<TRequest, TResponse> : PaginationRequestHandler<TRequest, TResponse> where TRequest : SearchRequest, IRequest<TResponse>
    {
        public SearchRequestHandler(DatabaseService dbService, IMapper mapper, IHttpContextAccessor httpContext, CurrentContext userAccessor) : base(dbService, mapper, httpContext, userAccessor)
        {
        }
    }
}

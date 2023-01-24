using AutoMapper;
using MediatR;
using YZPortal.Core.Domain.Contexts;

namespace YZPortal.API.Infrastructure.Mediatr
{
    public abstract class BaseRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        protected PortalContext Database { get; }
        protected CurrentContext CurrentContext { get; set; }
        protected HttpContext? HttpContext { get; set; }
        protected IMapper Mapper { get; set; }

        public BaseRequestHandler(PortalContext dbContext, IMapper mapper, IHttpContextAccessor httpContext, CurrentContext currentContext)
        {
            Database = dbContext;
            Mapper = mapper;
            HttpContext = httpContext.HttpContext ?? null;
            CurrentContext = currentContext;
        }

        public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
    }
}

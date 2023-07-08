using AutoMapper;
using MediatR;
using YZPortal.Core.Domain.Contexts;
using YZPortal.Core.Domain.Database;

namespace YZPortal.API.Infrastructure.Mediatr
{
    public abstract class BaseRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        protected DatabaseService DatabaseService { get; set; }
        protected CurrentContext CurrentContext { get; set; }
        protected HttpContext? HttpContext { get; set; }
        protected IMapper Mapper { get; set; }

        public BaseRequestHandler(DatabaseService dbService, IMapper mapper, IHttpContextAccessor httpContext, CurrentContext currentContext)
        {
            DatabaseService = dbService;
            Mapper = mapper;
            HttpContext = httpContext.HttpContext ?? null;
            CurrentContext = currentContext;
        }

        public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
    }
}

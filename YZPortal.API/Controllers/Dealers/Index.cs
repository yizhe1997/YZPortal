using AutoMapper;
using System.Net;
using YZPortal.API.Infrastructure.Mediatr;
using YZPortal.Core.Domain.Contexts;
using YZPortal.Core.Domain.Database.Dealers;
using YZPortal.Core.Error;

namespace YZPortal.Api.Controllers.Dealers
{
    public class Index
    {
        public class Request : SearchRequest<SearchResponse<Model>>
        {
        }

        public class Model : DealersViewModel
        {
        }

        public class RequestHandler : SearchRequestHandler<Request, SearchResponse<Model>>
        {
            public RequestHandler(PortalContext dbContext, IMapper mapper, IHttpContextAccessor httpContext, CurrentContext userAccessor) : base(dbContext, mapper, httpContext, userAccessor)
            {
            }

            public override async Task<SearchResponse<Model>> Handle(Request request, CancellationToken cancellationToken)
            {
                var query = CurrentContext.CurrentUserDealers;

                return await CreateIndexResponseAsync<Dealer, Model>(
                    request,
                    query,
                     x => x.Name.Contains(request.SearchString));
            }
        }
    }
}

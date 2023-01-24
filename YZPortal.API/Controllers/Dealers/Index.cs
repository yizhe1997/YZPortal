using AutoMapper;
using YZPortal.API.Infrastructure.Mediatr;
using YZPortal.Core.Domain.Contexts;
using YZPortal.Core.Domain.Database.Dealers;

namespace YZPortal.Api.Controllers.Dealers
{
    public class Index
    {
        public class Request : SearchRequest<SearchResponse<Model>>
        {
            public bool IsCurrentContextFilterEnabled { get; set; }
            public Guid AzureAdTokenSubClaim { get; set; }
            public Guid AzureAdB2CTokenSubClaim { get; set; }
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
                var query = request.IsCurrentContextFilterEnabled ? Database.Dealers: CurrentContext.Dealers;

                if (request.AzureAdTokenSubClaim != Guid.Empty && request.AzureAdB2CTokenSubClaim == Guid.Empty)
                    query = query.Join(
                        Database.Memberships.Where(m => m.User.AzureAdTokenSubClaim == request.AzureAdTokenSubClaim && m.Disabled == false),
                        d => d.Id, m => m.Dealer.Id, (d, m) => d);
                else if (request.AzureAdB2CTokenSubClaim != Guid.Empty && request.AzureAdTokenSubClaim == Guid.Empty)
                    query = query.Join(
                        Database.Memberships.Where(m => m.User.AzureAdB2CTokenSubClaim == request.AzureAdB2CTokenSubClaim && m.Disabled == false),
                        d => d.Id, m => m.Dealer.Id, (d, m) => d);

                return await CreateIndexResponseAsync<Dealer, Model>(
                    request,
                    query,
                     x => x.Name.Contains(request.SearchString));
            }
        }
    }
}

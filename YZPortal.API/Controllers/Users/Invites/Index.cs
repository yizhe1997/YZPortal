using AutoMapper;
using YZPortal.API.Controllers.Pagination;
using YZPortal.API.Infrastructure.Mediatr;
using YZPortal.Core.Domain.Contexts;
using YZPortal.Core.Domain.Database.Users;

namespace YZPortal.API.Controllers.Users.Invites
{
    public class Index
    {
        public class Request : SearchRequest<SearchResponse<Model>>
        {
        }
        public class Model : InviteViewModel
        {
        }
        public class RequestHandler : SearchRequestHandler<Request, SearchResponse<Model>>
        {
            public RequestHandler(PortalContext dbContext, IMapper mapper, IHttpContextAccessor httpContext, CurrentContext userAccessor) : base(dbContext, mapper, httpContext, userAccessor)
            {
            }
            public override async Task<SearchResponse<Model>> Handle(Request request, CancellationToken cancellationToken)
            {
                return await CreateIndexResponseAsync<UserInvite, Model>(
                    request,
                    Database.UserInvites.Where(y => y.ClaimedDateTime == null),
                    x => x.Email.Contains(request.SearchString));
            }
        }
    }
}

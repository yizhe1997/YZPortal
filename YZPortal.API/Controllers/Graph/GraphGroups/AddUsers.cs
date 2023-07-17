using AutoMapper;
using MediatR;
using YZPortal.API.Infrastructure.Mediatr;
using YZPortal.Core.Domain.Contexts;
using YZPortal.Core.Domain.Database;
using YZPortal.Core.Graph;
using YZPortal.FullStackCore.Models.Abstracts;

namespace YZPortal.API.Controllers.Graph.GraphGroups
{
    public class AddUsers
    {
        public class Request : IRequest<BaseResponseModel>
        {
            public string[] UserSubjectIds { get; set; } = Array.Empty<string>();
            public string? GroupId { get; set; }
        }
        public class RequestHandler : BaseRequestHandler<Request, BaseResponseModel>
        {
            private readonly GraphClientProvider _graphClientProvider;

            public RequestHandler(DatabaseService dbService, IMapper mapper, IHttpContextAccessor httpContext, CurrentContext userAccessor, GraphClientProvider graphClientProvider) : base(dbService, mapper, httpContext, userAccessor)
            {
                _graphClientProvider = graphClientProvider;
            }
            public override async Task<BaseResponseModel> Handle(Request request, CancellationToken cancellationToken)
            {
                await _graphClientProvider.GroupAddUsersAsync(request.GroupId, request.UserSubjectIds, cancellationToken);

                return new BaseResponseModel() { };
            }
        }
    }
}

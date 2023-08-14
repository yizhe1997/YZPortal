using AutoMapper;
using MediatR;
using YZPortal.API.Infrastructure.Mediatr;
using YZPortal.Core.Domain.Contexts;
using YZPortal.Core.Domain.Database;
using YZPortal.FullStackCore.Models.Users.Configs;
using YZPortal.FullStackCore.Requests.Users.Configs;

namespace YZPortal.API.Controllers.Users.Configs
{
    public class UpdatePortalConfig
    {
        public class Request : UpdatePortalConfigRequest, IRequest<PortalConfigModel>
        {
            public string? UserSubjectId { get; set; }
        }
        public class RequestHandler : BaseRequestHandler<Request, PortalConfigModel>
        {
            public RequestHandler(DatabaseService dbService, IMapper mapper, IHttpContextAccessor httpContext, CurrentContext userAccessor) : base(dbService, mapper, httpContext, userAccessor)
            {
            }
            public override async Task<PortalConfigModel> Handle(Request request, CancellationToken cancellationToken)
            {
                var portalConfig = await DatabaseService.UpdatePortalConfigAsync(request.UserSubjectId, request, cancellationToken);

                // Return mapped model
                return Mapper.Map<PortalConfigModel>(portalConfig);
            }
        }
    }
}

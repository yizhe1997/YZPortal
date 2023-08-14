using AutoMapper;
using MediatR;
using YZPortal.API.Infrastructure.Mediatr;
using YZPortal.Core.Domain.Contexts;
using YZPortal.Core.Domain.Database;
using YZPortal.FullStackCore.Models.Users.Configs;

namespace YZPortal.API.Controllers.Users.Configs
{
    public class Index
    {
        public class Request : IRequest<ConfigsModel>
        {
            public string? UserSubjectId { get; set; }
        }
        public class RequestHandler : BaseRequestHandler<Request, ConfigsModel>
        {
            public RequestHandler(DatabaseService dbService, IMapper mapper, IHttpContextAccessor httpContext, CurrentContext userAccessor) : base(dbService, mapper, httpContext, userAccessor)
            {
            }
            public override async Task<ConfigsModel> Handle(Request request, CancellationToken cancellationToken)
            {
                // Get all of user's configs
                var configs = await DatabaseService.GetConfigsAsync(request.UserSubjectId, cancellationToken);

                // Return mapped model
                return Mapper.Map<ConfigsModel>(configs);
            }
        }
    }
}

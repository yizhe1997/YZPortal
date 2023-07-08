using AutoMapper;
using MediatR;
using System.ComponentModel.DataAnnotations;
using YZPortal.API.Infrastructure.Mediatr;
using YZPortal.Core.Domain.Contexts;
using YZPortal.Core.Domain.Database;
using YZPortal.Core.Graph;

namespace YZPortal.API.Controllers.Graph.GraphGroups
{
    public class DisplayNamesForUser
    {
        public class Request : IRequest<Model>
        {
            [Required]
            public string? ObjectId { get; set; }
        }
        public class Model
        {
            public string[]? GroupDisplayNames { get; set; }
        }
        public class RequestHandler : BaseRequestHandler<Request, Model>
        {
            private readonly GraphClientProvider _graphClientProvider;

            public RequestHandler(DatabaseService dbService, IMapper mapper, IHttpContextAccessor httpContext, CurrentContext userAccessor, GraphClientProvider graphClientProvider) : base(dbService, mapper, httpContext, userAccessor)
            {
                _graphClientProvider = graphClientProvider;
            }
            public override async Task<Model> Handle(Request request, CancellationToken cancellationToken)
            {
                var userGroups = await _graphClientProvider.UserGroupsGetAsync(request.ObjectId, new string[] { "displayName" });
                var userGroupDisplayNames = userGroups.Select(x => x.DisplayName ?? string.Empty).ToArray() ?? Array.Empty<string>();

                return new Model() { GroupDisplayNames = userGroupDisplayNames };
            }
        }
    }
}

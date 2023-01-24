using AutoMapper;
using MediatR;
using System.ComponentModel.DataAnnotations;
using YZPortal.API.Infrastructure.Mediatr;
using YZPortal.Core.Domain.Contexts;
using YZPortal.Core.Domain.Database.Dealers;

namespace YZPortal.Api.Controllers.Dealers
{
    public class Create
    {
        public class Request : IRequest<Model>
        {
            [Required]
            public string? Name { get; set; }
        }

        public class Model : DealersViewModel
        {
        }

        public class RequestHandler : BaseRequestHandler<Request, Model>
        {
            public RequestHandler(PortalContext dbContext, IMapper mapper, IHttpContextAccessor httpContext, CurrentContext userAccessor) : base(dbContext, mapper, httpContext, userAccessor)
            {
            }
            public override async Task<Model> Handle(Request request, CancellationToken cancellationToken)
            {
                var newDealer = Mapper.Map<Dealer>(request);

                Database.Dealers.Add(newDealer);

                await Database.SaveChangesAsync();

                return Mapper.Map<Model>(newDealer);
            }
        }
    }
}

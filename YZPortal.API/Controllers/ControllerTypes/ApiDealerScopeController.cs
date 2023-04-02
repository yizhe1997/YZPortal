using MediatR;
using Microsoft.AspNetCore.Authorization;
using YZPortal.FullStackCore.Infrastructure.Security.Authorization;

namespace YZPortal.API.Controllers.ControllerTypes
{
    [Authorize(Policy = Policies.Dealer)]
    public class ApiDealerScopeController : ApiSecureController
    {
        public ApiDealerScopeController(IMediator mediator, LinkGenerator linkGenerator) : base(mediator, linkGenerator)
        {
        }
    }
}

using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace YZPortal.API.Controllers.ControllerTypes
{
    [Authorize(Policy = "CombinedScheme")]
    public class ApiSecureCombineController : ApiBaseController
    {
        public ApiSecureCombineController(IMediator mediator, LinkGenerator linkGenerator) : base(mediator, linkGenerator)
        {
        }
    }
}

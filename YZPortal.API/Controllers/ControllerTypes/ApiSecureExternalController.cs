using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web.Resource;

namespace YZPortal.API.Controllers.ControllerTypes
{
    [Authorize(Policy = "AllAuthenSchemes")]
	[RequiredScope(scopeRequiredByAPI)]
    public class ApiSecureExternalController : ApiBaseController
    {
        const string scopeRequiredByAPI = "Authorization";
        public ApiSecureExternalController(IMediator mediator, LinkGenerator linkGenerator) : base(mediator, linkGenerator)
        {
        }
    }
}

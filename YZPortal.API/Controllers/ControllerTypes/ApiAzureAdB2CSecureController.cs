using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;

namespace YZPortal.API.Controllers.ControllerTypes
{
    [Authorize(AuthenticationSchemes = Constants.AzureAdB2C)]
    [RequiredScope(scopeRequiredByAPI)]
    public class ApiAzureAdB2CSecureController : ApiBaseController
    {
        const string scopeRequiredByAPI = "Authorization";
        public ApiAzureAdB2CSecureController(IMediator mediator, LinkGenerator linkGenerator) : base(mediator, linkGenerator)
        {
        }
    }
}

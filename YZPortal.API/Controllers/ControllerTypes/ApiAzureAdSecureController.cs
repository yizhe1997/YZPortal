using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;

namespace YZPortal.API.Controllers.ControllerTypes
{
    [Authorize(AuthenticationSchemes = Constants.AzureAd)]
    [RequiredScope(scopeRequiredByAPI)]
    public class ApiAzureAdSecureController : ApiBaseController
    {
        const string scopeRequiredByAPI = "Authorization";
        public ApiAzureAdSecureController(IMediator mediator, LinkGenerator linkGenerator) : base(mediator, linkGenerator)
        {
        }
    }
}

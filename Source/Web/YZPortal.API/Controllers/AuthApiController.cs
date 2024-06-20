using Application.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;

namespace YZPortal.API.Controllers
{
    [Authorize(AuthenticationSchemes = Constants.AzureAdB2C)]
    [RequiredScope(ScopeConstants.APIAccess)]
    public class AuthApiController(IMediator mediator, LinkGenerator linkGenerator) : ApiControllerBase(mediator, linkGenerator)
    {
    }
}

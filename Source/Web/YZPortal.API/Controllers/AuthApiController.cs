using Application.Constants;
using Application.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;

namespace YZPortal.API.Controllers
{
    [Authorize(AuthenticationSchemes = Constants.AzureAdB2C)]
    [RequiredScope(ScopeConstants.APIAccess)]
    public class AuthApiController : ApiControllerBase
    {
        public AuthApiController(IMediator mediator, LinkGenerator linkGenerator) : base(mediator, linkGenerator)
        {
        }

        [HttpPost("TriggerApiControllerException")]
        public async Task<ActionResult<Result>> TriggerApiControllerExceptionAsync()
        {
            throw new Exception();

            return Ok();
        }
    }
}

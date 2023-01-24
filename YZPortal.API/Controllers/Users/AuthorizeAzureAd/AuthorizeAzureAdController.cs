using MediatR;
using Microsoft.AspNetCore.Mvc;
using YZPortal.API.Controllers.ControllerTypes;

namespace YZPortal.API.Controllers.Users.AuthorizeAzureAd
{
    public class AuthorizeAzureAdController : ApiAzureAdSecureController
    {
        public AuthorizeAzureAdController(IMediator mediator, LinkGenerator linkGenerator) : base(mediator, linkGenerator)
        {
        }

        [HttpPost]
        public async Task<ActionResult<Create.Model>> PostAuthorizeAzureAd([FromBody] Create.Request request) =>
            await _mediator.Send(request);
    }
}

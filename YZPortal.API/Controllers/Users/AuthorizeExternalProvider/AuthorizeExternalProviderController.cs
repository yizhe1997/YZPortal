using MediatR;
using Microsoft.AspNetCore.Mvc;
using YZPortal.API.Controllers.ControllerTypes;

namespace YZPortal.API.Controllers.Users.AuthorizeExternalProvider
{
    public class AuthorizeExternalProviderController : ApiSecureExternalController
    {
        public AuthorizeExternalProviderController(IMediator mediator, LinkGenerator linkGenerator) : base(mediator, linkGenerator)
        {
        }

        /// <summary>
        ///     Determines the access rights of a user authenticated externally for a given dealer.
        /// </summary>
        /// <response code="200">User successfully authorized</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost]
        public async Task<ActionResult<Create.Model>> PostAuthorizeAzureAd([FromBody] Create.Request request) =>
            await _mediator.Send(request);
    }
}

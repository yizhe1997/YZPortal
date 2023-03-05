using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YZPortal.API.Controllers.ControllerTypes;

namespace YZPortal.API.Controllers.Users.Authenticate
{
    public class AuthenticateController : ApiBaseController
    {
        public AuthenticateController(IMediator mediator, LinkGenerator linkGenerator) : base(mediator, linkGenerator)
        {
        }

		/// <summary>
		///     Verifies the identity of a user.
		/// </summary>
		/// <response code="200">User successfully authentcated</response>
		[ProducesResponseType(StatusCodes.Status200OK)]
		[AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<Create.Model>> PostAuthenticate([FromBody] Create.Request request) =>
            await _mediator.Send(request);
    }
}

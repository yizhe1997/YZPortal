using MediatR;
using Microsoft.AspNetCore.Mvc;
using YZPortal.API.Controllers.ControllerTypes;

namespace YZPortal.API.Controllers.Users.Authorize
{
    public class AuthorizeController : ApiSecureController
    {
        public AuthorizeController(IMediator mediator, LinkGenerator linkGenerator) : base(mediator, linkGenerator)
        {
        }

		/// <summary>
		///     Determines the access rights of a user for a given dealer.
		/// </summary>
		/// <response code="200">User successfully authorized</response>
		[ProducesResponseType(StatusCodes.Status200OK)]
		[HttpPost]
        public async Task<ActionResult<Create.Model>> PostAuthorize([FromBody] Create.Request request) =>
            await _mediator.Send(request);

    }
}

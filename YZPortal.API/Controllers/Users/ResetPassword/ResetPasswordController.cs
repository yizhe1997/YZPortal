using MediatR;
using Microsoft.AspNetCore.Mvc;
using YZPortal.API.Controllers.ControllerTypes;

namespace YZPortal.API.Controllers.Users.ResetPassword
{
    public class PasswordResetController : ApiBaseController
    {
        public PasswordResetController(IMediator mediator, LinkGenerator linkGenerator) : base(mediator, linkGenerator)
        {
        }

		/// <summary>
		///     Creates an emailable password reset item for a user
		/// </summary>
		[HttpPost]
        public async Task<ActionResult<Create.Model>> PostResetPassword([FromBody] Create.Request request) =>
            await _mediator.Send(request);

		/// <summary>
		///     Claim the password reset item emailed to a user.
		/// </summary>
		[HttpPost("claim")]
        public async Task<ActionResult<Claim.Model>> ClaimResetPassword([FromBody] Claim.Request request) =>
            await _mediator.Send(request);
    }
}

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YZPortal.API.Controllers.ControllerTypes;
using YZPortal.API.Controllers.Pagination;

namespace YZPortal.API.Controllers.Dealers.Invites
{
	public class InvitesController : ApiDealerScopeController
	{
		public InvitesController(IMediator mediator, LinkGenerator linkGenerator) : base(mediator, linkGenerator)
		{ }

		/// <summary>
		///     Creates an emailable membership invite for new user or membership notification for 
		///     existing user in the current dealer.
		/// </summary>
		[HttpPost]
		public async Task<ActionResult<Create.Model>> PostInvite([FromBody] Create.Request request) =>
			await _mediator.Send(request);

		/// <summary>
		///     Claims a membership invite and creates a membership for a user in the current dealer.
		/// </summary>
		[AllowAnonymous]
		[HttpPost("Claim")]
		public async Task<ActionResult<Claim.Model>> ClaimInvite([FromBody] Claim.Request request) =>
			await _mediator.Send(request);

		/// <summary>
		///     Returns a list of unclaimed membership invite for the current dealer.
		/// </summary>
		[HttpGet]
		public async Task<ActionResult<SearchResponse<Index.Model>>> GetInvites([FromQuery] Index.Request request) =>
			await _mediator.Send(request);

		/// <summary>
		///     Deletes a user's membership invite for the current dealer.
		/// </summary>
		[HttpDelete("{id}")]
		public async Task<ActionResult<Delete.Model>> DeleteInvite([FromRoute] Guid id) =>
			await _mediator.Send(new Delete.Request { Id = id });
	}
}

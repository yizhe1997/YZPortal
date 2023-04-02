using MediatR;
using Microsoft.AspNetCore.Mvc;
using YZPortal.API.Controllers.ControllerTypes;
using YZPortal.API.Controllers.Memberships;
using YZPortal.API.Controllers.Pagination;

namespace YZPortal.Api.Controllers.Memberships
{
    public class MembershipsController : ApiDealerScopeController
	{
        public MembershipsController(IMediator mediator, LinkGenerator linkGenerator) : base(mediator, linkGenerator)
        { 
        }

		/// <summary>
		///     Returns a list of membership for the current dealer.
		/// </summary>
		[HttpGet]
        public async Task<ActionResult<SearchResponse<Index.Model>>> GetMemberships([FromQuery] Index.Request request) =>
        await _mediator.Send(request);

		/// <summary>
		///     Returns a user's membership detail for the current dealer.
		/// </summary>
		[HttpGet("{id}")]
        public async Task<ActionResult<Details.Model>> GetMembership([FromRoute] Guid id, [FromQuery] Details.Request request)
        {
            request.Id = id;
            return await _mediator.Send(request);
        }

		/// <summary>
		///     Creates a membership for a user in the current dealer.
		/// </summary>
		[HttpPost]
        public async Task<ActionResult<Create.Model>> CreateMembership([FromBody] Create.Request request) =>
            await _mediator.Send(request);

		/// <summary>
		///     Updates the detail of a user's membership for the current dealer.
		/// </summary>
		[HttpPut("{id}")]
        public async Task<ActionResult<Update.Model>> UpdateMembership([FromRoute] Guid id, [FromBody] Update.Request request)
        {
            request.Id = id;
            return await _mediator.Send(request);
        }

		/// <summary>
		///     Deletes a user's membership for the current dealer.
		/// </summary>
		[HttpDelete("{id}")]
        public async Task<ActionResult<Delete.Model>> DeleteMembership([FromRoute] Guid id) => 
            await _mediator.Send(new Delete.Request { Id = id });
    }
}

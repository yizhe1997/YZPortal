using MediatR;
using Microsoft.AspNetCore.Mvc;
using YZPortal.API.Controllers.ControllerTypes;
using YZPortal.API.Controllers.Pagination;

namespace YZPortal.API.Controllers.Users
{
	public class UsersController : ApiAdminScopeControllerClass
	{
		public UsersController(IMediator mediator, LinkGenerator linkGenerator) : base(mediator, linkGenerator)
		{
		}

		/// <summary>
		///     Returns a list of user.
		/// </summary>
		[HttpGet]
		public async Task<ActionResult<SearchResponse<Index.Model>>> GetUser([FromQuery] Index.Request request) => await _mediator.Send(request);

		/// <summary>
		///     Deletes a user.
		/// </summary>
		[HttpDelete("{id}")]
		public async Task<ActionResult<Delete.Model>> DeleteUser([FromRoute] Guid id) => await _mediator.Send(new Delete.Request { Id = id });

		/// <summary>
		///     Returns user detail.
		/// </summary>
		[HttpGet("{id}")]
		public async Task<ActionResult<Details.Model>> GetUser([FromRoute] Guid id, [FromQuery] Details.Request request)
		{
			request.Id = id;
			return await _mediator.Send(request);
		}

		/// <summary>
		///     Updates the detail of a user.
		/// </summary>
		[HttpPut("{id}")]
		public async Task<ActionResult<Update.Model>> UpdateUser([FromRoute] Guid id, [FromBody] Update.Request request)
		{
			if (id != Guid.Empty) request.Id = id;
			return await _mediator.Send(request);
		}
	}
}

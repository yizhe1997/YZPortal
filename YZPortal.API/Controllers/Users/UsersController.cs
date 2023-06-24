using MediatR;
using Microsoft.AspNetCore.Mvc;
using YZPortal.API.Controllers.ControllerTypes;
using YZPortal.FullStackCore.Models.Abstracts;

namespace YZPortal.API.Controllers.Users
{
    public class UsersController : ApiB2CController
    {
		public UsersController(IMediator mediator, LinkGenerator linkGenerator) : base(mediator, linkGenerator)
		{
		}

		/// <summary>
		///     Returns a list of user.
		/// </summary>
		[HttpGet]
		public async Task<ActionResult<SearchModel<Index.Model>>> GetUsers([FromQuery] Index.Request request) => await _mediator.Send(request);

		/// <summary>
		///     Deletes a user.
		/// </summary>
		[HttpDelete("{subjectId}")]
		public async Task<ActionResult<Delete.Model>> DeleteUser([FromRoute] Guid subjectId) => await _mediator.Send(new Delete.Request { Id = subjectId });

		/// <summary>
		///     Returns user detail.
		/// </summary>
		[HttpGet("{subjectId}")]
		public async Task<ActionResult<Details.Model>> GetUser([FromRoute] Guid subjectId, [FromQuery] Details.Request request)
		{
			request.SubjectId = subjectId;
			return await _mediator.Send(request);
		}

        /// <summary>
        ///     Creates new user via b2c token.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] Create.Request request)
        {
            var payLoad = await _mediator.Send(request);

            return CreatedAtAction(nameof(GetUser), new { id = payLoad.Id }, payLoad);
        }

        /// <summary>
        ///     Updates the detail of a user via b2c token.
        /// </summary>
        [HttpPut("{subjectId}")]
		public async Task<ActionResult<Update.Model>> UpdateUser([FromRoute] Guid subjectId, [FromBody] Update.Request request)
		{
			if (subjectId != Guid.Empty) request.SubjectId = subjectId;
			return await _mediator.Send(request);
		}
	}
}

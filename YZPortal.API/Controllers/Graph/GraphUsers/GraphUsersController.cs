using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YZPortal.API.Controllers.ControllerTypes;
using YZPortal.API.Controllers.Pagination;

namespace YZPortal.API.Controllers.Graph.GraphUsers
{
	public class GraphUsersController : ApiAdminScopeControllerClass
	{
		public GraphUsersController(IMediator mediator, LinkGenerator linkGenerator) : base(mediator, linkGenerator)
		{
		}
		
        [AllowAnonymous]
        [HttpGet("Graph")]
		public async Task<ActionResult<SearchResponse<Index.Model>>> GetGraphUsers([FromQuery] Index.Request request) => await _mediator.Send(request);

	}
}

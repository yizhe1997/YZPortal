using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YZPortal.API.Controllers.ControllerTypes;
using YZPortal.FullStackCore.Infrastructure.Security.Authorization;
using YZPortal.FullStackCore.Models.Abstracts;
using YZPortal.FullStackCore.Models.Graph.Users;

namespace YZPortal.API.Controllers.Graph.GraphUsers
{
    public class GraphUsersController : ApiB2CController
    {
		public GraphUsersController(IMediator mediator, LinkGenerator linkGenerator) : base(mediator, linkGenerator)
		{	
		}

        [Authorize(Policy = Policies.Administrator)]
        [HttpGet("Graph")]
		public async Task<ActionResult<SearchModel<GraphUserModel>>> GetGraphUsers([FromQuery] Index.Request request) => await _mediator.Send(request);

	}
}

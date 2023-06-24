using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YZPortal.API.Controllers.ControllerTypes;
using YZPortal.FullStackCore.Infrastructure.Security.Authorization;
using YZPortal.FullStackCore.Models.Abstracts;

namespace YZPortal.API.Controllers.Graph.GraphUsers
{
    public class GraphUsersController : ApiB2CController
    {
		public GraphUsersController(IMediator mediator, LinkGenerator linkGenerator) : base(mediator, linkGenerator)
		{	
		}

        [Authorize(Policy = Policies.Administrator)]
        [HttpGet("Graph")]
		public async Task<ActionResult<SearchModel<Index.Model>>> GetGraphUsers([FromQuery] Index.Request request) => await _mediator.Send(request);

	}
}

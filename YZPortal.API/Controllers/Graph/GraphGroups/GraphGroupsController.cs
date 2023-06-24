using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YZPortal.API.Controllers.ControllerTypes;
using YZPortal.FullStackCore.Models.Abstracts;

namespace YZPortal.API.Controllers.Graph.GraphGroups
{
    public class GraphGroupsController : ApiB2CController
    {
        public GraphGroupsController(IMediator mediator, LinkGenerator linkGenerator) : base(mediator, linkGenerator)
        {
        }

        [HttpGet]
        public async Task<ActionResult<SearchModel<Index.Model>>> GetGraphGroups([FromQuery] Index.Request request) => await _mediator.Send(request);

        [Authorize(AuthenticationSchemes = "Basic")]
        [HttpGet("DisplayNames")]
        public async Task<ActionResult<DisplayNamesForUser.Model>> GetGraphGroupDisplayNamesForUser([FromQuery] DisplayNamesForUser.Request request) => await _mediator.Send(request);
    }
}

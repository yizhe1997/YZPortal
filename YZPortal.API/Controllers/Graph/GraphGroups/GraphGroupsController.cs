using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using YZPortal.API.Controllers.ControllerTypes;
using YZPortal.API.Controllers.Pagination;

namespace YZPortal.API.Controllers.Graph.GraphGroups
{
    public class GraphGroupsController : ApiBaseController
    {
        public GraphGroupsController(IMediator mediator, LinkGenerator linkGenerator) : base(mediator, linkGenerator)
        {
        }

        [Authorize(AuthenticationSchemes = "Basic")]
        [HttpGet]
        public async Task<ActionResult<SearchResponse<Index.Model>>> GetGraphGroups([FromQuery] Index.Request request) => await _mediator.Send(request);

        [Authorize(AuthenticationSchemes = "Basic")]
        [HttpGet("DisplayNames")]
        public async Task<ActionResult<DisplayNamesForUser.Model>> GetGraphGroupDisplayNamesForUser([FromQuery] DisplayNamesForUser.Request request) => await _mediator.Send(request);
    }
}

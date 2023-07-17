using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;
using YZPortal.API.Controllers.ControllerTypes;
using YZPortal.FullStackCore.Models.Abstracts;
using YZPortal.FullStackCore.Models.Graph.Groups;

namespace YZPortal.API.Controllers.Graph.GraphGroups
{
    public class GraphGroupsController : ApiBaseController
    {
        private const string _ = "API.Access";

        public GraphGroupsController(IMediator mediator, LinkGenerator linkGenerator) : base(mediator, linkGenerator)
        {
        }

        [Authorize(AuthenticationSchemes = Constants.AzureAdB2C)]
        [RequiredScope(_)]
        [HttpGet]
        public async Task<ActionResult<SearchModel<GraphGroupModel>>> GetGraphGroups([FromQuery] Index.Request request) => await _mediator.Send(request);

        [Authorize(AuthenticationSchemes = Constants.AzureAdB2C)]
        [RequiredScope(_)]
        [HttpPost("AddUser")]
        public async Task<ActionResult<BaseResponseModel>> AddUserToGraphGroup([FromBody] AddUsers.Request request)
        {
            return await _mediator.Send(request);
        }

        [Authorize(AuthenticationSchemes = Constants.AzureAdB2C)]
        [RequiredScope(_)]
        [HttpPost("RemoveUser")]
        public async Task<ActionResult<BaseResponseModel>> RemoveUserFromGraphGroup([FromBody] RemoveUser.Request request)
        {
            return await _mediator.Send(request);
        }

        [Authorize(AuthenticationSchemes = "Basic")]
        [HttpGet("DisplayNames")]
        public async Task<ActionResult<DisplayNamesForUser.Model>> GetGraphGroupDisplayNamesForUser([FromQuery] DisplayNamesForUser.Request request) => await _mediator.Send(request);
    }
}

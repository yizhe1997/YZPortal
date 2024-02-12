using Application.Interfaces.Services;
using Application.Models.Graph;
using Application.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Requests.Graph.Groups;
using Microsoft.Identity.Web.Resource;
using Microsoft.Identity.Web;

namespace YZPortal.API.Controllers.Graph
{
    public class GraphGroupsController : ApiControllerBase
    {
        private readonly IGraphService _graphService;
        private const string _ = "API.Access";

        public GraphGroupsController(IGraphService graphService, IMediator mediator, LinkGenerator linkGenerator) : base(mediator, linkGenerator)
        {
            _graphService = graphService;
        }

        [Authorize(AuthenticationSchemes = Constants.AzureAdB2C)]
        [RequiredScope(_)]
        [HttpGet]
        public async Task<ActionResult<SearchResult<GroupModel>>> GetGraphGroups([FromQuery] GetGraphGroupsRequest request)
        {
            var response = string.IsNullOrEmpty(request.UserSubId) ?
                await _graphService.GroupsToSearchResultAsync(request) :
                await _graphService.UserGroupsToSearchResultAsync(request.UserSubId, request);

            return Ok(response);
        }

        [Authorize(AuthenticationSchemes = Constants.AzureAdB2C)]
        [RequiredScope(_)]
        [HttpPost("AddUser")]
        public async Task<ActionResult<Result>> AddUserToGraphGroup([FromBody] AddUsersToGroupCommand request)
        {
            var response = await _graphService.GroupAddUsersAsync(request);
            return Ok(response);
        }

        [Authorize(AuthenticationSchemes = Constants.AzureAdB2C)]
        [RequiredScope(_)]
        [HttpPost("RemoveUser")]
        public async Task<ActionResult<Result>> RemoveUserFromGraphGroup([FromBody] RemoveUserFromGroupCommand request)
        {
            var response = await _graphService.GroupRemoveUserAsync(request);
            return Ok(response);
        }

        [Authorize(AuthenticationSchemes = "Basic")]
        [HttpGet("DisplayNames")]
        public async Task<IActionResult> GetGraphGroupDisplayNamesForUser([FromQuery] string ObjectId)
        {
            var response = await _graphService.UserGroupDisplayNamesGetAsync(ObjectId);

            return Ok(new
            {
                GroupDisplayNames = response
            });
        }
    }
}

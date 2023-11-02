using Application.Interfaces.Services;
using Application.Models.Graph;
using Application.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Requests.Graph.Groups;

namespace YZPortal.API.Controllers.Graph
{
    public class GraphGroupsController : ControllerBase
    {
        private readonly IGraphService _graphService;

        public GraphGroupsController(IGraphService graphService, IMediator mediator, LinkGenerator linkGenerator) : base(mediator, linkGenerator)
        {
            _graphService = graphService;
        }

        [HttpGet]
        public async Task<ActionResult<SearchResult<GroupModel>>> GetGraphGroups([FromQuery] GetGraphGroupsRequest request)
        {
            var response = string.IsNullOrEmpty(request.UserSubId) ?  
                await _graphService.GroupsToSearchResultAsync(request) : 
                await _graphService.UserGroupsToSearchResultAsync(request.UserSubId, request);

            return Ok(response);
        }

        [HttpPost("AddUser")]
        public async Task<ActionResult<Result>> AddUserToGraphGroup([FromBody] AddUsersToGroupRequest request)
        {
            var response = await _graphService.GroupAddUsersAsync(request);
            return Ok(response);
        }

        [HttpPost("RemoveUser")]
        public async Task<ActionResult<Result>> RemoveUserFromGraphGroup([FromBody] RemoveUserFromGroupRequest request)
        {
            var response = await _graphService.GroupRemoveUserAsync(request);
            return Ok(response);
        }

        [Authorize(AuthenticationSchemes = "Basic")]
        [HttpGet("DisplayNames")]
        public async Task<ActionResult<string[]>> GetGraphGroupDisplayNamesForUser([FromQuery] string ObjectId)
        {
            var response = await _graphService.UserGroupDisplayNamesGetAsync(ObjectId);
            return Ok(response);
        }
    }
}

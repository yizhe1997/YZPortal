using Application.Interfaces.Services;
using Application.Models.Graph;
using Application.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Requests.Graph.Groups;
using Microsoft.Identity.Web.Resource;
using Microsoft.Identity.Web;
using Application.Constants;

namespace YZPortal.API.Controllers.Graph
{
    public class GraphGroupsController(IGraphService graphService, ICacheService cache, IMediator mediator, LinkGenerator linkGenerator) : ApiControllerBase(mediator, linkGenerator)
    {
        [Authorize(AuthenticationSchemes = Constants.AzureAdB2C)]
        [RequiredScope(ScopeConstants.APIAccess)]
        [HttpGet]
        public async Task<ActionResult<SearchResult<GroupModel>>> GetGraphGroups([FromQuery] GetGraphGroupsRequest request, CancellationToken cancellationToken)
        {
            var response = string.IsNullOrEmpty(request.UserSubId) ?
                await cache.GetOrSetAsync(nameof(GetGraphGroups), () => graphService.GroupsToSearchResultAsync(request), cancellationToken) :
                await graphService.UserGroupsToSearchResultAsync(request.UserSubId, request, cancellationToken: cancellationToken);

            return Ok(response);
        }

        [Authorize(AuthenticationSchemes = Constants.AzureAdB2C)]
        [RequiredScope(ScopeConstants.APIAccess)]
        [HttpPost("AddUser")]
        public async Task<ActionResult<Result>> AddUserToGraphGroup([FromBody] AddUsersToGroupCommand request, CancellationToken cancellationToken)
        {
            var response = await graphService.GroupAddUsersAsync(request, cancellationToken);
            return Ok(response);
        }

        [Authorize(AuthenticationSchemes = Constants.AzureAdB2C)]
        [RequiredScope(ScopeConstants.APIAccess)]
        [HttpPost("RemoveUser")]
        public async Task<ActionResult<Result>> RemoveUserFromGraphGroup([FromBody] RemoveUserFromGroupCommand request, CancellationToken cancellationToken)
        {
            var response = await graphService.GroupRemoveUserAsync(request, cancellationToken);
            return Ok(response);
        }

        [Authorize(AuthenticationSchemes = "Basic")]
        [HttpGet("DisplayNames")]
        public async Task<IActionResult> GetGraphGroupDisplayNamesForUser([FromQuery] string ObjectId, CancellationToken cancellationToken)
        {
            var response = await graphService.UserGroupDisplayNamesGetAsync(ObjectId, cancellationToken: cancellationToken);

            return Ok(new
            {
                GroupDisplayNames = response
            });
        }
    }
}

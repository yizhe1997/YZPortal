using Application.Interfaces.Services;
using Application.Models;
using Application.Models.Graph;
using Application.Requests.Indexes;
using Domain.Enums.Memberships;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace YZPortal.API.Controllers.Graph
{
    public class GraphUsersController(IGraphService graphService, IMediator mediator, LinkGenerator linkGenerator) : AuthApiController(mediator, linkGenerator)
    {
        [Authorize(Policy = nameof(Role.Administrator))]
        [HttpGet("Graph")]
        public async Task<ActionResult<SearchResult<UserModel>>> GetGraphUsers([FromQuery] SearchRequest request, CancellationToken cancellationToken)
        {
            var response = await graphService.UsersToSearchResultAsync(request);
            return Ok(response);
        }
    }
}

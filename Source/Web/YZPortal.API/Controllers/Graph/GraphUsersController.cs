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
    public class GraphUsersController : ControllerBase
    {
        private readonly IGraphService _graphService;

        public GraphUsersController(IGraphService graphService, IMediator mediator, LinkGenerator linkGenerator) : base(mediator, linkGenerator)
        {
            _graphService = graphService;
        }

        [Authorize(Policy = nameof(DealerRoleNames.Administrator))]
        [HttpGet("Graph")]
        public async Task<ActionResult<SearchResult<UserModel>>> GetGraphUsers([FromQuery] SearchRequest request)
        {
            var response = await _graphService.UsersToSearchResultAsync(request);
            return Ok(response);
        }

    }
}

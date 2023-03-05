using MediatR;
using Microsoft.AspNetCore.Mvc;
using YZPortal.API.Controllers.ControllerTypes;
using YZPortal.API.Controllers.Pagination;

namespace YZPortal.API.Controllers.Memberships.ContentAccessLevels
{
    public class ContentAccessLevelsController : ApiSecureController
    {
        public ContentAccessLevelsController(IMediator mediator, LinkGenerator linkGenerator) : base(mediator, linkGenerator)
        {
        }

        /// <summary>
        ///     Returns a list of plausible content access level for membership.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<SearchResponse<Index.Model>>> GetContentAccessLevels([FromQuery] Index.Request request) =>
            await _mediator.Send(request);
    }
}

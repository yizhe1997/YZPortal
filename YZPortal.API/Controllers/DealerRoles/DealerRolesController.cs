using MediatR;
using Microsoft.AspNetCore.Mvc;
using YZPortal.API.Controllers.ControllerTypes;
using YZPortal.API.Controllers.Pagination;

namespace YZPortal.Api.Controllers.DealerRoles
{
    public class DealerRolesController : ApiSecureController
    {
        public DealerRolesController(IMediator mediator, LinkGenerator linkGenerator) : base(mediator, linkGenerator)
        {
        }

		/// <summary>
		///     Returns a list of plausible dealer role for membership.
		/// </summary>
		[HttpGet]
        public async Task<ActionResult<SearchResponse<Index.Model>>> GetDealerRoles([FromQuery] Index.Request request) =>
            await _mediator.Send(request);
    }
}


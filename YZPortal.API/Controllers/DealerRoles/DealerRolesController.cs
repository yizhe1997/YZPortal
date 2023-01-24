using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace YZPortal.Api.Controllers.DealerRoles
{
    public class DealerRolesController : ApiSecureController
    {
        public DealerRolesController(IMediator mediator, LinkGenerator linkGenerator) : base(mediator, linkGenerator)
        {
        }

        [HttpGet]
        public async Task<ActionResult<SearchResponse<Index.Model>>> GetDealerRoles([FromQuery] Index.Request request) =>
            await _mediator.Send(request);
    }
}


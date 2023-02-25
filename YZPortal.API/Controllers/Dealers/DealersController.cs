using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YZPortal.API.Controllers.ControllerTypes;
using YZPortal.API.Infrastructure.Mediatr;

namespace YZPortal.Api.Controllers.Dealers
{
    public class DealersController : ApiBaseController
	{
        public DealersController(IMediator mediator, LinkGenerator linkGenerator) : base(mediator, linkGenerator)
        {
        }

        // add scope required admin
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		[HttpPost]
        public async Task<ActionResult<Create.Model>> CreateDealer([FromBody] Create.Request request) =>
            await _mediator.Send(request);

		[Authorize(Policy = "AllAuthenSchemes")]
		[HttpGet]
        public async Task<ActionResult<SearchResponse<Index.Model>>> GetDealers([FromQuery] Index.Request request) =>
            await _mediator.Send(request);
    }
}

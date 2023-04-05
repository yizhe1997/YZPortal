using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YZPortal.API.Controllers.ControllerTypes;
using YZPortal.API.Controllers.Pagination;
using YZPortal.FullStackCore.Infrastructure.Security.Authorization;

namespace YZPortal.Api.Controllers.Dealers
{
    public class DealersController : ApiBaseController
	{
        public DealersController(IMediator mediator, LinkGenerator linkGenerator) : base(mediator, linkGenerator)
        {
        }

		/// <summary>
		///     Creates a new dealer
		/// </summary>
		[Authorize(Policy = Policies.IsAdmin, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		[HttpPost]
        public async Task<ActionResult<Create.Model>> CreateDealer([FromBody] Create.Request request) =>
            await _mediator.Send(request);

		/// <summary>
		///     Returns a list of dealer assigned to the current user.
		/// </summary>
		/// <param name="tokenSubClaim">External token</param>
		[Authorize(Policy = "AllAuthenSchemes")]
		[HttpGet]
        public async Task<ActionResult<SearchResponse<Index.Model>>> GetDealers([FromQuery] Index.Request request) =>
            await _mediator.Send(request);
    }
}

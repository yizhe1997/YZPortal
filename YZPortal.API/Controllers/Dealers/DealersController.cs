using MediatR;
using Microsoft.AspNetCore.Mvc;
using YZPortal.API.Controllers.ControllerTypes;
using YZPortal.API.Infrastructure.Mediatr;

namespace YZPortal.Api.Controllers.Dealers
{
    public class DealersController : ApiSecureCombineController
    {
        public DealersController(IMediator mediator, LinkGenerator linkGenerator) : base(mediator, linkGenerator)
        {
        }

        [HttpPost]
        public async Task<ActionResult<Create.Model>> CreateDealer([FromBody] Create.Request request) =>
            await _mediator.Send(request);

        [HttpGet]
        public async Task<ActionResult<SearchResponse<Index.Model>>> GetDealers([FromQuery] Index.Request request) =>
            await _mediator.Send(request);
    }
}

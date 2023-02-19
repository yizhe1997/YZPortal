using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YZPortal.API.Controllers.ControllerTypes;
using YZPortal.API.Infrastructure.Mediatr;

namespace YZPortal.API.Controllers.Memberships.Invites
{
    public class InvitesController : ApiDealerScopeController
    {
        public InvitesController(IMediator mediator, LinkGenerator linkGenerator) : base(mediator, linkGenerator)
        { }

        [HttpPost]
        public async Task<ActionResult<Create.Model>> PostInvite([FromBody] Create.Request request) =>
            await _mediator.Send(request);

        [AllowAnonymous]
        [HttpPost("Claim")]
        public async Task<ActionResult<Claim.Model>> ClaimInvite([FromBody] Claim.Request request) =>
            await _mediator.Send(request);

        [HttpGet]
        public async Task<ActionResult<SearchResponse<Index.Model>>> GetInvites([FromQuery] Index.Request request) =>
            await _mediator.Send(request);

        [HttpDelete("{id}")]
        public async Task<ActionResult<Delete.Model>> DeleteInvite([FromRoute] Guid id) =>
            await _mediator.Send(new Delete.Request { Id = id });
    }
}

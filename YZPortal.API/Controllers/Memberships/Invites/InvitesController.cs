using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        [HttpPost("claim")]
        public async Task<ActionResult<Claim.Model>> ClaimInvite([FromBody] Claim.Request request) =>
            await _mediator.Send(request);

        [HttpGet]
        public async Task<ActionResult<SearchResponse<Index.Model>>> GetInvites([FromQuery] Index.Request request) =>
            await _mediator.Send(request);

        [HttpDelete("{id}")]
        public async Task<ActionResult<Delete.Model>> DeleteInvite([FromRoute] Guid id) =>
            await _mediator.Send(new Delete.Request { Id = id });

        [HttpPost("import")]
        public async Task<ActionResult<Import.Model>> ImportInvites([FromQuery] string callbackUrl, [FromForm] IFormFile file)
        {
            return await _mediator.Send(new Import.Request { file = file, CallbackUrl = callbackUrl });
        }

        [HttpGet("importXLSM")]
        public async Task<ActionResult<InviteXLSM.Model>> GetImportInvitesXLSM([FromQuery] InviteXLSM.Request request)
        {
            var model = await _mediator.Send(request);
            return File(model.Stream, model.MimeType, model.FileName);
        }

        [AllowAnonymous]
        [HttpGet("validate/{token}")]
        public async Task<ActionResult<ValidateToken.Model>> ValideInviteToken([FromRoute] Guid token) =>
            await _mediator.Send(new ValidateToken.Request { Token = token });
    }
}

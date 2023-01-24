using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace YZPortal.Api.Controllers.Memberships
{
    public class MembershipsController : ApiCurrentDealerScopeController
    {
        // Constructor
        public MembershipsController(IMediator mediator, LinkGenerator linkGenerator) : base(mediator, linkGenerator)
        { 
        }

        [HttpGet]
        public async Task<ActionResult<SearchResponse<Index.Model>>> GetMemberships([FromQuery] Index.Request request) =>
        await _mediator.Send(request);

        [HttpGet("{id}")]
        public async Task<ActionResult<Details.Model>> GetMembership([FromRoute] Guid id, [FromQuery] Details.Request request)
        {
            request.Id = id;
            return await _mediator.Send(request);
        }

        [HttpPost("Invite")]
        public async Task<ActionResult<Create.Model>> CreateMembership([FromBody] Create.Request request) =>
            await _mediator.Send(request);

        [HttpPost("Invite/Bulk")]
        public async Task<ActionResult<CreateBulk.Model>> ImportInvites([FromQuery] string callbackUrl, [FromForm] IFormFile file)
        {
            return await _mediator.Send(new CreateBulk.Request { file = file, CallbackUrl = callbackUrl });
        }

        [HttpGet("Invite/Sheet")]
        public async Task<ActionResult<CreateBulkSheet.Model>> GetInvitesExcelSheet([FromBody] CreateBulkSheet.Request request)
        {
            var model = await _mediator.Send(request);
            return File(model.Stream, model.MimeType, model.FileName);
        }

        [AllowAnonymous]
        [HttpPost("Invite/Claim")]
        public async Task<ActionResult<Claim.Model>> ClaimMembership([FromBody] Claim.Request request) =>
            await _mediator.Send(request);

        [HttpPut("{id}")]
        public async Task<ActionResult<Update.Model>> UpdateMembership([FromRoute] Guid id, [FromBody] Update.Request request)
        {
            request.Id = id;
            return await _mediator.Send(request);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Delete.Model>> DeleteMembership([FromRoute] Guid id) => 
            await _mediator.Send(new Delete.Request { Id = id });
    }
}

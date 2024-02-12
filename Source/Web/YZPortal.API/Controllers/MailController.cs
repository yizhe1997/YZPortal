using Application.Interfaces.Services.Mailing;
using Application.Models;
using Application.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;

namespace YZPortal.API.Controllers
{
    [Authorize(AuthenticationSchemes = Constants.AzureAdB2C)]
    [RequiredScope(_)]
    public class MailController : ApiControllerBase
    {
        private readonly IMailService _mailService;
        private const string _ = "API.Access";

        public MailController(IMailService mailService, IMediator mediator, LinkGenerator linkGenerator) : base(mediator, linkGenerator)
        {
            _mailService = mailService;
        }

        [HttpPost]
        public async Task<ActionResult<Result>> CreateMail([FromForm] CreateMailCommand command)
        {
            await _mailService.SendAsync(command);

            return Ok();
        }
    }
}

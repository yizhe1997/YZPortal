using Application.Interfaces.Services.Mailing;
using Application.Models;
using Application.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace YZPortal.API.Controllers
{
    public class MailController : AuthApiController
    {
        private readonly IMailService _mailService;

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

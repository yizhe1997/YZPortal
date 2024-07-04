using Application.Interfaces.Services.Mailing;
using Application.Models;
using Application.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace YZPortal.API.Controllers
{
    public class MailController(IMailService mailService, IMediator mediator, LinkGenerator linkGenerator) : AuthApiController(mediator, linkGenerator)
    {
        [HttpPost]
        public async Task<ActionResult<Result>> CreateMail([FromForm] CreateMailCommand command)
        {
            await mailService.SendAsync(command);

            return Ok();
        }
    }
}

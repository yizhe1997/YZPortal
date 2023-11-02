using Application.Features.Users.Configs.Commands.UpdatePortalConfig;
using Application.Features.Users.Configs.Queries.GetConfigs;
using Application.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;

namespace YZPortal.API.Controllers.Users.Configs
{
    [Authorize(AuthenticationSchemes = Constants.AzureAdB2C)]
    [RequiredScope(_)]
    public class ConfigsController : ControllerBase
    {
        private const string _ = "API.Access";

        public ConfigsController(IMediator mediator, LinkGenerator linkGenerator) : base(mediator, linkGenerator)
        {
        }

        /// <summary>
        /// Updates the portal configuration of the current user.
        /// </summary>
        [HttpPut("portalConfiguration/{userSubId}")]
        public async Task<ActionResult<Result>> UpdatePortalConfiguration([FromRoute] string userSubId, [FromBody] UpdateUserPortalConfigCommand command)
        {
            command.UserSubId = userSubId;

            var response = await _mediator.Send(command);

            return Ok(response);
        }

        /// <summary>
        /// Get portal configurations for current user
        /// </summary>
        [HttpGet("{userSubId}")]
        public async Task<ActionResult<Result<ConfigsDto>>> GetPortalConfigurations([FromRoute] string userSubId, [FromQuery] GetUserConfigsQuery query)
        {
            query.UserSubId = userSubId;

            var response = await _mediator.Send(query);

            return Ok(response);
        }
    }
}

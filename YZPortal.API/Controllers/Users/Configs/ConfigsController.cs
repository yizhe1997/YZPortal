using MediatR;
using Microsoft.AspNetCore.Mvc;
using YZPortal.API.Controllers.ControllerTypes;
using YZPortal.FullStackCore.Models.Users.Configs;

namespace YZPortal.API.Controllers.Users.Configs
{
    public class ConfigsController : ApiB2CController
    {
        public ConfigsController(IMediator mediator, LinkGenerator linkGenerator) : base(mediator, linkGenerator)
        {
        }

        /// <summary>
        ///     Updates the portal configuration of a user.
        /// </summary>
        [HttpPut("portalConfiguration/{userSubjectId}")]
        public async Task<ActionResult<PortalConfigModel>> UpdatePortalConfiguration([FromRoute] string userSubjectId, [FromBody] UpdatePortalConfig.Request request)
        {
            request.UserSubjectId = userSubjectId;
            return await _mediator.Send(request);
        }

        /// <summary>
        ///     Get configurations for current user
        /// </summary>
        [HttpGet("{userSubjectId}")]
        public async Task<ActionResult<ConfigsModel>> GetPortalConfigurations([FromRoute] string userSubjectId, [FromQuery] Index.Request request)
        {
            request.UserSubjectId = userSubjectId;
            return await _mediator.Send(request);
        }
    }
}

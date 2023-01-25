﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using YZPortal.API.Controllers.ControllerTypes;
using YZPortal.API.Infrastructure.Mediatr;

namespace YZPortal.Api.Controllers.ContentAccessLevels
{
    public class ContentAccessLevelsController : ApiSecureController
    {
        public ContentAccessLevelsController(IMediator mediator, LinkGenerator linkGenerator) : base(mediator, linkGenerator)
        {
        }

        [HttpGet]
        public async Task<ActionResult<SearchResponse<Index.Model>>> GetContentAccessLevels([FromQuery] Index.Request request) =>
            await _mediator.Send(request);
    }
}

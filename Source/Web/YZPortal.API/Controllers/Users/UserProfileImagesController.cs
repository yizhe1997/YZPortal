﻿using Application.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Features.Users.Configs.Queries.GetConfigs;
using Application.Features.Users.UserProfileImages.Commands;

namespace YZPortal.API.Controllers.Users
{
    public class UserProfileImagesController : AuthApiController
    {
        public UserProfileImagesController(IMediator mediator, LinkGenerator linkGenerator) : base(mediator, linkGenerator)
        {
        }

        /// <summary>
        /// Upload user profile image
        /// </summary>
        [HttpPut("{userId}")]
        public async Task<ActionResult<Result>> UploadUserProfileImageAsync([FromRoute] Guid userId, [FromBody] UploadUserProfileImageCommand command)
        {
            command.RefId = userId;

            var response = await _mediator.Send(command);

            return Ok(response);
        }

        /// <summary>
        /// Delete user profile image
        /// </summary>
        [HttpGet("{userId}")]
        public async Task<ActionResult<Result<ConfigsDto>>> DeleteUserProfileImageAsync([FromRoute] Guid userId)
        {
            var response = await _mediator.Send(new DeleteUserProfileImageCommand()
            {
                UserId = userId
            });

            return Ok(response);
        }

        /// <summary>
        /// Download user profile image
        /// </summary>
        [HttpGet("{userId}")]
        public async Task<ActionResult<Result<ConfigsDto>>> DownloadUserProfileImageAsync([FromRoute] Guid userId)
        {
            var response = await _mediator.Send(new DownloadUserProfileImageCommand()
            {
                UserId = userId
            });

            return Ok(response);
        }
    }
}
using Application.Features.Users.UserProfileImages.Commands;
using Application.Interfaces.Services;
using Application.Interfaces.Services.Identity;
using Application.Models;
using Application.Models.Identity;
using Application.Requests.Indexes;
using Application.Requests.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace YZPortal.API.Controllers.Users
{
    public class UsersController(IGraphService graphService, IUserService userService, ICurrentUserService currentUserService, IMediator mediator, LinkGenerator linkGenerator) : AuthApiController(mediator, linkGenerator)
    {
        [HttpPost("TriggerApiControllerException")]
        public async Task<ActionResult<Result>> TriggerApiControllerExceptionAsync()
        {
            throw new Exception();
        }

        /// <summary>
        /// Returns a list of application user.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<SearchResult<UserModel>>> GetUsers([FromQuery] SearchRequest request, CancellationToken cancellationToken)
        {
            var response = await userService.GetSearchResultAsync(request);
            return Ok(response);
        }

        /// <summary>
        /// Deletes application user.
        /// </summary>
        [HttpDelete("{userSubId}")]
        public async Task<ActionResult<Result>> DeleteUser([FromRoute] string userSubId, CancellationToken cancellationToken)
        {
            var response = await userService.DeleteBySubIdAsync(userSubId, cancellationToken);

            // TODO: Move to scheduler 
            await graphService.UserDeleteAsync(userSubId, cancellationToken);

            return Ok(response);
        }

        /// <summary>
        /// Gets application user detail.
        /// </summary>
        [HttpGet("{userSubId}")]
        public async Task<ActionResult<Result<UserModel>>> GetUser([FromRoute] string userSubId, CancellationToken cancellationToken)
        {
            var response = await userService.GetBySubIdAsync(userSubId, cancellationToken);

            return Ok(response);
        }

        /// <summary>
        /// Create a new application user associated with the current user context.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Result>> CreateUser(CancellationToken cancellationToken)
        {
            var response = await userService.CreateAsync(currentUserService, cancellationToken);

            return CreatedAtAction(nameof(GetUser), new { subjectId = currentUserService.NameIdentifier }, response);
        }

        // TODO: properly handle transaction scope... https://stackoverflow.com/questions/36636272/transactions-with-asp-net-identity-usermanager
        /// <summary>
        /// Updates the details of the application user.
        /// </summary>
        [HttpPut("{userSubId}")]
        public async Task<ActionResult<Result>> UpdateUser([FromRoute] string userSubId, [FromBody] UpdateUserCommand request, CancellationToken cancellationToken)
        {
            await userService.UpdateAsync(userSubId, request, cancellationToken);

            var response = await graphService.UserUpdateAsync(userSubId, request, cancellationToken);

            return Ok(response);
        }

        [HttpPut(nameof(UpdateCurrentUserViaHttpContext))]
        public async Task<ActionResult<Result>> UpdateCurrentUserViaHttpContext(CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteIdentityCommand() { UserSubId = currentUserService.NameIdentifier }, cancellationToken);

            var response = await userService.UpdateAsync(currentUserService.NameIdentifier, currentUserService, cancellationToken);

            return Ok(response);
        }
    }
}

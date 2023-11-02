using Application.Interfaces.Services;
using Application.Interfaces.Services.Identity;
using Application.Models;
using Application.Models.Identity;
using Application.Requests.Indexes;
using Application.Requests.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using Microsoft.Identity.Web;

namespace YZPortal.API.Controllers.Users
{
    [Authorize(AuthenticationSchemes = Constants.AzureAdB2C)]
    [RequiredScope(_)]
    public class UsersController : ControllerBase
    {
        private readonly IGraphService _graphService;
        private readonly IUserService _userService;
        private readonly ICurrentUserService _currentUserService;
        private const string _ = "API.Access";

        public UsersController(IGraphService graphService, IUserService userService, ICurrentUserService currentUserService, IMediator mediator, LinkGenerator linkGenerator) : base(mediator, linkGenerator)
        {
            _userService = userService;
            _currentUserService = currentUserService;
            _graphService = graphService;
        }

        /// <summary>
        /// Returns a list of application user.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<SearchResult<UserModel>>> GetUsers([FromQuery] SearchRequest request)
        {
            var response = await _userService.GetSearchResultAsync(request);
            return Ok(response);
        }

        /// <summary>
        /// Deletes application user.
        /// </summary>
        [HttpDelete("{userSubId}")]
        public async Task<ActionResult<Result>> DeleteUser([FromRoute] string userSubId)
        {
            var response = await _userService.DeleteBySubIdAsync(userSubId);

            // TODO: Move to scheduler 
            await _graphService.UserDeleteAsync(userSubId);

            return Ok(response);
        }

        /// <summary>
        /// Gets application user detail.
        /// </summary>
        [HttpGet("{userSubId}")]
        public async Task<ActionResult<Result<UserModel>>> GetUser([FromRoute] string userSubId)
        {
            var response = await _userService.GetBySubIdAsync(userSubId);

            return Ok(response);
        }

        /// <summary>
        /// Create a new application user associated with the current user context.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> CreateUser()
        {
            var response = await _userService.CreateAsync(_currentUserService);

            return CreatedAtAction(nameof(GetUser), new { subjectId = _currentUserService.NameIdentifier }, response);
        }

        /// <summary>
        /// Updates the details of the application user.
        /// </summary>
        [HttpPut("{userSubId}")]
        public async Task<ActionResult<Result>> UpdateUser([FromRoute] string userSubId, [FromBody] UpdateUserRequest request)
        {
            var response = await _graphService.UserUpdateAsync(userSubId, request);

            // TODO: Move to scheduler
            // TODO: also use transaction scope if this is not last step since https://stackoverflow.com/questions/36636272/transactions-with-asp-net-identity-usermanager
            await _userService.UpdateAsync(userSubId, request);

            return Ok(response);
        }

        [HttpPut(nameof(UpdateCurrentUserViaHttpContext))]
        public async Task<ActionResult<Result>> UpdateCurrentUserViaHttpContext()
        {
            var response = await _userService.UpdateAsync(_currentUserService.NameIdentifier, _currentUserService);

            return Ok(response);
        }
    }
}

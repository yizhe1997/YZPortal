using MediatR;
using Microsoft.AspNetCore.Authorization;
using YZPortal.FullStackCore.Infrastructure.Security.Authorization;

namespace YZPortal.API.Controllers.ControllerTypes
{
	[Authorize(Policy = Policies.IsAdmin)]
	public class ApiAdminScopeControllerClass : ApiSecureController
	{
		public ApiAdminScopeControllerClass(IMediator mediator, LinkGenerator linkGenerator) : base(mediator, linkGenerator)
		{
		}
	}
}

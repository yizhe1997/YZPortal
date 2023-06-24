using MediatR;
using Microsoft.AspNetCore.Authorization;
using YZPortal.FullStackCore.Infrastructure.Security.Authorization;

namespace YZPortal.API.Controllers.ControllerTypes
{
	[Authorize(Policy = Policies.Administrator)]
	public class ApiAdminScopeControllerClass : ApiJwtBearerController
	{
		public ApiAdminScopeControllerClass(IMediator mediator, LinkGenerator linkGenerator) : base(mediator, linkGenerator)
		{
		}
	}
}

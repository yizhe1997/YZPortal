using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;

namespace YZPortal.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    [Authorize(AuthenticationSchemes = Constants.AzureAdB2C)]
    [RequiredScope(_)]
    public class ControllerBase : Controller
    {
        protected readonly IMediator _mediator;
        protected readonly LinkGenerator _linkGenerator;
        private const string _ = "API.Access";

        public ControllerBase(IMediator mediator, LinkGenerator linkGenerator)
        {
            _mediator = mediator;
            _linkGenerator = linkGenerator;
        }
    }
}

using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace YZPortal.API.Controllers.ControllerTypes
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public abstract class ApiBaseController : Controller
    {
        protected readonly IMediator _mediator;
        protected readonly LinkGenerator _linkGenerator;

        public ApiBaseController(IMediator mediator, LinkGenerator linkGenerator)
        {
            _mediator = mediator;
            _linkGenerator = linkGenerator;
        }
    }
}

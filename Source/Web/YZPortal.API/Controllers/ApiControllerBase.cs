using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace YZPortal.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class ApiControllerBase : Controller
    {
        protected readonly IMediator _mediator;
        protected readonly LinkGenerator _linkGenerator;

        public ApiControllerBase(IMediator mediator, LinkGenerator linkGenerator)
        {
            _mediator = mediator;
            _linkGenerator = linkGenerator;
        }
    }
}

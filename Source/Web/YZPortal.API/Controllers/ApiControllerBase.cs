using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace YZPortal.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class ApiControllerBase(IMediator mediator, LinkGenerator linkGenerator) : Controller
    {
        protected readonly IMediator _mediator = mediator;
        protected readonly LinkGenerator _linkGenerator = linkGenerator;
    }
}

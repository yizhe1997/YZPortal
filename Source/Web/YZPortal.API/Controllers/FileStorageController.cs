using Application.Interfaces.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web.Resource;
using Microsoft.Identity.Web;
using Microsoft.AspNetCore.Mvc;
using Application.Models;
using Application.Requests;

namespace YZPortal.API.Controllers
{
    [Authorize(AuthenticationSchemes = Constants.AzureAdB2C)]
    [RequiredScope(_)]
    public class FileStorageController : ApiControllerBase
    {
        private readonly IFileStorageService _fileStorageService;
        private const string _ = "API.Access";

        public FileStorageController(IFileStorageService fileStorageService, IMediator mediator, LinkGenerator linkGenerator) : base(mediator, linkGenerator)
        {
            _fileStorageService = fileStorageService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> DownloadFile([FromRoute] Guid id)
        {
            var result = await _fileStorageService.DownloadFileAsync(id);

            return new FileStreamResult(result.Data.Stream, result.Data.ContentType)
            {
                FileDownloadName = result.Data.FileName,
            };
        }

        [HttpPost]
        public async Task<ActionResult<Result>> UploadFile([FromForm] UploadFileCommand command)
        {
            var result = await _fileStorageService.UploadFileAsync(command);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Result>> DeleteFile([FromRoute] Guid id) 
        {
            var result = await _fileStorageService.DeleteFileAsync(id);

            return Ok(result);
        }
    }
}

using Microsoft.AspNetCore.Http;

namespace Application.Requests
{
    public abstract class UploadDataFileCommand
    {
        public Guid RefId { get; set; }
        public IFormFile? File { get; set; }
        public string? FileName { get; set; }
    }
}

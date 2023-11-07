using Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Application.Requests
{
    public class UploadFileCommand
    {
        public Guid RefId { get; set; }
        public IFormFile? File { get; set; }
        public FileTypes FileType { get; set; }
        public string? FileName { get; set; }
    }
}

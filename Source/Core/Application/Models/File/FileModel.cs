using Application.Models.Abstracts;

namespace Application.Models.File
{
    public class FileModel : AuditableModel<Guid>
    {
        public string? Name { get; set; }
        public string? Type { get; set; }
    }
}

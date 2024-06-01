using Domain.Entities.Auditable;

namespace Domain.Entities.Misc
{
    public abstract class DataFile : AuditableEntity<Guid>
    {
        public string? Name { get; set; }
        public string? ContentType { get; set; }
        public long Size { get; set; }
        public string? ContainerName { get; set; }
        public abstract Guid RefId { get; set; }
        public string? Url { get; set; }
    }
}

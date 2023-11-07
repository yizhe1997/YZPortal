using Domain.Entities.Auditable;
using Domain.Entities.Users;

namespace Domain.Entities.Misc
{
    public class File : AuditableEntity<Guid>
    {
        public string? Name { get; set; }
        public string? ContentType { get; set; }
        public long Size { get; set; }
        public string? ContainerName { get; set; }
        public int RefType { get; set; }
        public Guid RefId { get; set; }

        // Navigation property to represent the relationship to entities
        public User? User { get; set; }
    }
}

using YZPortal.Core.Domain.Database.EntityTypes.Auditable;

namespace YZPortal.Worker.Tasks.Email
{
    public class EmailMessage
    {
        public IEnumerable<EmailableEntity> Notifications { get; set; } = new List<EmailableEntity>();
        public string? Subject { get; set; }
        public string? Content { get; set; }
    }
}

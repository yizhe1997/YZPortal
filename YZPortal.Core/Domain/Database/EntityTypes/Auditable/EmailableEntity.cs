using System.ComponentModel.DataAnnotations;

namespace YZPortal.Core.Domain.Database.EntityTypes.Auditable
{
    public abstract class EmailableEntity : AuditableEntity
    {
        [Required]
        public string? Email { get; set; }
        public DateTime? SentDateTime { get; set; }
        public string? FailedMessage { get; set; }
        public DateTime? FailedSentDateTime { get; set; }
        public int Attempts { get; set; }
        public DateTime? LastAttemptedSentDateTime { get; set; }
    }
}

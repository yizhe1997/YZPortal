using Domain.Entities.Auditable;

namespace Domain.Entities.Sync
{
    public class SyncStatus : AuditableEntity<Guid>
    {
        public int Name { get; set; }
        public bool Status { get; set; }
        public string? Notes { get; set; }
        public int Type { get; set; }
        public bool IsSyncDisabled { get; set; }
        public DateTime ExecutionDateTime { get; set; }
    }
}

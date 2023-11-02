namespace Domain.Entities.Auditable
{
    public abstract class AuditableEntity<TId> : BaseEntity<TId>, IAuditableEntity<TId>
    {
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
    }
}

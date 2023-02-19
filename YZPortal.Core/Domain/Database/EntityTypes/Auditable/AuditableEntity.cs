namespace YZPortal.Core.Domain.Database.EntityTypes.Auditable
{
    public abstract class AuditableEntity : BaseEntity
    {
        public DateTime CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
    }
}

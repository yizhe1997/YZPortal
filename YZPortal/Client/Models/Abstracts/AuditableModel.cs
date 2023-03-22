namespace YZPortal.Client.Models.Abstracts
{
    public abstract class AuditableModel : BaseModel
    {
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}

namespace YZPortal.FullStackCore.Models.Abstracts
{
    public class BaseEnumModel : AuditableModel
    {
        public int Code { get; set; }
        public string? Name { get; set; }
    }
}

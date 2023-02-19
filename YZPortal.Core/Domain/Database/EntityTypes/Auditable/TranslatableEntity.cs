namespace YZPortal.Core.Domain.Database.EntityTypes.Auditable
{
    public class TranslatableEntity : AuditableEntity
    {
        public string LanguageCode { get; set; } = "en";
    }
}

namespace YZPortal.Core.Domain.Database.EntityTypes
{
    public class TranslatableEntity : AuditableEntity
    {
        public string LanguageCode { get; set; } = "en";
    }
}

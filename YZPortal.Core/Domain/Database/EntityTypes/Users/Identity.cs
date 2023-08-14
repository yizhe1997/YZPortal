namespace YZPortal.Core.Domain.Database.EntityTypes.Users
{
    public class Identity : BaseEntity
    {
        public string? Issuer { get; set; }
        public string? IssuerUserId { get; set; }
    }
}

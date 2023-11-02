namespace Domain.Entities.Users
{
    public class Identity : BaseEntity<Guid>
    {
        public string? Issuer { get; set; }
        public string? IssuerUserId { get; set; }
    }
}

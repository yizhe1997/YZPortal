using YZPortal.Core.Domain.Database.EntityTypes;

namespace YZPortal.Core.Domain.Database.Users
{
    public class Identity : BaseEntity
    {
        public string? Issuer { get; set; }
        public string? IssuerUserId { get; set; }
    }
}

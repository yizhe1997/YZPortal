using YZPortal.FullStackCore.Models.Abstracts;

namespace YZPortal.FullStackCore.Models.Users
{
    public class IdentityModel : BaseModel
    {
        public string? Issuer { get; set; }
        public string? IssuerUserId { get; set; }
    }
}

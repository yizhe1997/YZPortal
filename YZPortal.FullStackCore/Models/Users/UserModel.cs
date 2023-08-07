using YZPortal.FullStackCore.Models.Abstracts;

namespace YZPortal.FullStackCore.Models.Users
{
    public class UserModel : AuditableModel
    {
        public string? UserName { get; set; }

        #region B2C Claims

        public string? DisplayName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? SubjectIdentifier { get; set; }
        public string? IpAddress { get; set; }
        public DateTime? AuthTime { get; set; }
        public DateTime? AuthExpireTime { get; set; }
        public string? AuthClassRef { get; set; }
        public string? MobilePhone { get; set; }

        #region Identity Provider

        public string? IdentityProvider { get; set; } // TODO
        public string? LastidpAccessToken { get; set; } // TODO
        public List<IdentityModel> Identities { get; set; } = new List<IdentityModel>();

        #endregion

        #endregion
    }
}

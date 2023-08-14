using Microsoft.AspNetCore.Identity;
using YZPortal.Core.Domain.Database.EntityTypes.Auditable;
using YZPortal.Core.Domain.Database.EntityTypes.Users.Configs;

namespace YZPortal.Core.Domain.Database.EntityTypes.Users
{
    // Microsoft.AspNetCore.Identity Tables:
    //+------------------+------------------+------------------+
    //|      Table       |   Description    |       Used       |
    //+------------------+------------------+------------------+
    //| AspNetUsers      | The users.       | Yes              | 
    //| AspNetRoles      | The roles.       | No               | // TODO: enum and initialize via graph for first time....
    //| AspNetUserRoles  | Roles of users.  | No               | 
    //| AspNetUserClaims | Claims by users. | No               | 
    //| AspNetRoleClaims | Claims by roles. | No               | 
    //+------------------+------------------+------------------+

    public class User : IdentityUser<Guid>, IAuditableEntity
    {
        //public virtual ICollection<Logs> Logs { get; set; } = new HashSet<Logs>();

        // Ref: https://stackoverflow.com/questions/26430094/asp-net-identity-provider-signinmanager-keeps-returning-failure
        public override string? UserName { get { return Email; } set { Email = value; } } // TODO: fix sorting        

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

        public string? IdentityProvider { get; set; }
        public string? LastidpAccessToken { get; set; } // TODO: check how to do this for custom policy?
        public List<Identity> Identities { get; set; } = new List<Identity>();

        #endregion

        #endregion

        #region IAuditableEntity

        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }

        #endregion

        public PortalConfig? PortalConfig { get; set; }
    }
}

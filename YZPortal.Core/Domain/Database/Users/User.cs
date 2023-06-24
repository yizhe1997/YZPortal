using Microsoft.AspNetCore.Identity;
using YZPortal.Core.Domain.Database.EntityTypes.Auditable;

namespace YZPortal.Core.Domain.Database.Users
{
	// Microsoft.AspNetCore.Identity Tables:
	//+------------------+------------------+------------------+
	//|      Table       |   Description    |       Used       |
	//+------------------+------------------+------------------+
	//| AspNetUsers      | The users.       | Yes              | 
	//| AspNetRoles      | The roles.       | No               | 
	//| AspNetUserRoles  | Roles of users.  | No               | 
	//| AspNetUserClaims | Claims by users. | No               | 
	//| AspNetRoleClaims | Claims by roles. | No               | 
	//+------------------+------------------+------------------+

	public class User : IdentityUser<Guid>, IAuditableEntity
    {
        #region B2C Claims

        public string? DisplayName { get; set; }
        public Guid SubjectIdentifier { get; set; }
        public string? IpAddress { get; set; }
        public DateTime? AuthTime { get; set; }
        public DateTime? AuthExpireTime { get; set; }
        public string? AuthClassRef { get; set; }

        #region Identity Provider

        public string? IdentityProvider { get; set; } // TODO
        public string? LastidpAccessToken { get; set; } // TODO

        #endregion

        #endregion

        #region IAuditableEntity

        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }

        #endregion

        //public virtual ICollection<Logs> Logs { get; set; } = new HashSet<Logs>();

        // Ref: https://stackoverflow.com/questions/26430094/asp-net-identity-provider-signinmanager-keeps-returning-failure
        public override string? UserName { get { return Email; } set { Email = value; } } // TODO EMAIL        
    }
}

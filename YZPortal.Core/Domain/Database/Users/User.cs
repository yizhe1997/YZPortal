using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using YZPortal.Core.Domain.Database.Memberships;

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

	public class User : IdentityUser<Guid>
    {
        [Required]
        public string? Name { get; set; }
        public bool Admin { get; set; } = false;
		// Ref: https://stackoverflow.com/questions/26430094/asp-net-identity-provider-signinmanager-keeps-returning-failure
		public override string? UserName { get { return Email; } set { Email = value; } }
        public DateTime? LastLoggedIn { get; set; }
        public List<Membership> Memberships { get; set; } = new List<Membership>();
        public List<UserPasswordReset> UserPasswordResets { get; set; } = new List<UserPasswordReset>();
        public UserInvite? UserInvite { get; set; }

        #region Identity Provider

        public Guid TokenSubClaim { get; set; }
		public int IdentityProvider { get; set; }

		#endregion
	}
	[Flags]
	public enum IdentityProviderNames
	{
		None = 0,
		AzureAd = 1,
		AzureAdB2C = 2
	}
}

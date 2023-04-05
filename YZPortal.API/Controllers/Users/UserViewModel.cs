using YZPortal.API.Controllers.ViewModel.Auditable;

namespace YZPortal.API.Controllers.Users
{
	public class UserViewModel : AuditableViewModel
	{
		public string? Name { get; set; }
		public bool Admin { get; set; } = false;
		public string? UserName { get; set; }
		public DateTime? LastLoggedIn { get; set; }

		#region Identity Provider

		public Guid TokenSubClaim { get; set; }
		public int IdentityProvider { get; set; }

		#endregion
	}
}

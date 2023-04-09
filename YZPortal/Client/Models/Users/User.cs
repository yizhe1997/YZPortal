using YZPortal.Client.Models.Abstracts;

namespace YZPortal.Client.Models.Users
{
	public class User : AuditableModel
	{
		public string? Name { get; set; }
		public string? UserName { get; set; }
		public bool Admin { get; set; }
        public DateTime? LastLoggedIn { get; set; }
    }
}

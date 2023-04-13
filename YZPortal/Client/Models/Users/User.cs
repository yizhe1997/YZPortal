using System.ComponentModel.DataAnnotations;
using YZPortal.Client.Models.Abstracts;

namespace YZPortal.Client.Models.Users
{
	public class User : AuditableModel
    {
		public string? Name { get; set; }
        public string? UserName { get; set; } // fail to sort by because of get set 
		public bool Admin { get; set; }
        public DateTime? LastLoggedIn { get; set; }
    }
}

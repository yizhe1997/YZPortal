using System.ComponentModel.DataAnnotations;
using YZPortal.Core.Domain.Database.EntityTypes.Auditable;

namespace YZPortal.Core.Domain.Database.Users
{
    public class UserPasswordReset : EmailableEntity
    {
        public User? User { get; set; }
		[Required]
		public Guid UserId { get; set; }

		[Required]
        public string CallbackUrl { get; set; } = "{0}";
        public DateTime? ClaimedDateTime { get; set; }
        public DateTime? ValidUntilDateTime { get; set; } = DateTime.UtcNow.AddDays(3);

        [Required]
        public Guid Token { get; set; } = Guid.NewGuid();
    }
}

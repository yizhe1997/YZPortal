using System.ComponentModel.DataAnnotations;
using YZPortal.Core.Domain.Database.EntityTypes.Auditable;

namespace YZPortal.Core.Domain.Database.Memberships
{
    public class MembershipNotification : EmailableEntity
    {
        public Membership? Membership { get; set; }
		[Required]
		public Guid MembershipId { get; set; }
	}
}

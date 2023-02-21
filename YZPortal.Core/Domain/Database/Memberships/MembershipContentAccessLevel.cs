using System.ComponentModel.DataAnnotations;
using YZPortal.Core.Domain.Database.EntityTypes.Auditable;

namespace YZPortal.Core.Domain.Database.Memberships
{
    public class MembershipContentAccessLevel : AuditableEntity
    {
        public ContentAccessLevel? ContentAccessLevel { get; set; }
		[Required]
		public Guid ContentAccessLevelpId { get; set; }
		public Membership? Membership { get; set; }
		[Required]
		public Guid MembershipId { get; set; }

	}
}

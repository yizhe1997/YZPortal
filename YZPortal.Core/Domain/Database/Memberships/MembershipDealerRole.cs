using System.ComponentModel.DataAnnotations;
using YZPortal.Core.Domain.Database.EntityTypes.Auditable;

namespace YZPortal.Core.Domain.Database.Memberships
{
    /// <summary>
    ///     The list of membership for user role
    /// </summary>
    public class MembershipDealerRole : AuditableEntity
    {
        public DealerRole? DealerRole { get; set; }
		[Required]
		public Guid DealerRoleId { get; set; }
		public Membership? Membership { get; set; }
		[Required]
		public Guid MembershipId { get; set; }
	}
}

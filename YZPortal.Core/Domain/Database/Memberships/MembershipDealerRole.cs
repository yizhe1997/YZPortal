using System.ComponentModel.DataAnnotations;
using YZPortal.Core.Domain.Database.EntityTypes;

namespace YZPortal.Core.Domain.Database.Memberships
{
    /// <summary>
    ///     The list of membership for user role
    /// </summary>
    public class MembershipDealerRole : AuditableEntity
    {
        [Required]
        public DealerRole? DealerRole { get; set; }
        [Required]
        public Membership? Membership { get; set; }
    }
}

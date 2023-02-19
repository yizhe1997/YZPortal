using System.ComponentModel.DataAnnotations;
using YZPortal.Core.Domain.Database.EntityTypes.Auditable;

namespace YZPortal.Core.Domain.Database.Memberships
{
    public class DealerRole : AuditableEntity
    {
        [Required]
        public int Name { get; set; }
        public List<MembershipDealerRole> MembershipDealerRoles { get; set; } = new List<MembershipDealerRole>();
    }

    [Flags]
    public enum DealerRoleNames
    {
        None = 0,
        Admin = 1,
        User = 2,
        Representative = 3
    }
}

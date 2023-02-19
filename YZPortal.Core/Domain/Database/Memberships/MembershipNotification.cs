using System.ComponentModel.DataAnnotations;
using YZPortal.Core.Domain.Database.EntityTypes.Auditable;

namespace YZPortal.Core.Domain.Database.Memberships
{
    public class MembershipNotification : EmailableEntity
    {
        [Required]
        public Membership? Membership { get; set; }
    }
}

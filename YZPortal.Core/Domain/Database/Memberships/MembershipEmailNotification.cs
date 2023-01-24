using System.ComponentModel.DataAnnotations;
using YZPortal.Core.Domain.Database.EntityTypes;

namespace YZPortal.Core.Domain.Database.Memberships
{
    public class MembershipEmailNotification : EmailableEntity
    {
        [Required]
        public Membership? Membership { get; set; }
    }
}

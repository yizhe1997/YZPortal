using System.ComponentModel.DataAnnotations;
using YZPortal.Core.Domain.Database.EntityTypes;

namespace YZPortal.Core.Domain.Database.Memberships
{
    public class MembershipContentAccessLevel : AuditableEntity
    {
        [Required]
        public ContentAccessLevel? ContentAccessLevel { get; set; }
        [Required]
        public Membership? Membership { get; set; }
    }
}

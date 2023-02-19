using System.ComponentModel.DataAnnotations;
using YZPortal.Core.Domain.Database.EntityTypes.Auditable;

namespace YZPortal.Core.Domain.Database.Memberships
{
    public class ContentAccessLevel : AuditableEntity
    {
        [Required]
        public int Name { get; set; }
        // tis is wrong btw... just think about it
        public List<MembershipContentAccessLevel> MembershipContentAccessLevels { get; set; } = new List<MembershipContentAccessLevel>();
        // do a new table for the translation e.g eng/bob, cn/bobby uk/bobbie
    }

    [Flags]
    public enum ContentAccessLevelNames
    {
        None = 0,
        All = 1
    }
}

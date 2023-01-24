using System.ComponentModel.DataAnnotations;
using YZPortal.Core.Domain.Database.EntityTypes;

namespace YZPortal.Core.Domain.Database.Memberships
{
    public class ContentAccessLevel : AuditableEntity
    {
        [Required]
        public int Name { get; set; }
        public List<MembershipContentAccessLevel> MembershipContentAccessLevels { get; set; } = new List<MembershipContentAccessLevel>();
    }

    [Flags]
    public enum ContentAccessLevelNames
    {
        None = 0,
        All = 1
    }
}

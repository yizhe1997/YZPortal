using System.ComponentModel.DataAnnotations;
using YZPortal.Core.Domain.Database.Dealers;
using YZPortal.Core.Domain.Database.EntityTypes.Auditable;

namespace YZPortal.Core.Domain.Database.Users
{
    public class UserInviteDealerSelection : AuditableEntity
    {
        public Dealer? Dealer { get; set; }
        [Required]
        public Guid DealerId { get; set; }
        public int UserRole { get; set; }
        public int UserContentAccessLevels { get; set; }
    }
}

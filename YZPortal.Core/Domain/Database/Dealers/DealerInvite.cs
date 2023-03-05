using System.ComponentModel.DataAnnotations;
using YZPortal.Core.Domain.Database.Dealers;
using YZPortal.Core.Domain.Database.EntityTypes.Auditable;

namespace YZPortal.Core.Domain.Database.Memberships
{
    public class DealerInvite : EmailableEntity
    {
        [Required]
        public string? Name { get; set; }
        public Dealer? Dealer { get; set; }
        [Required]
        public Guid DealerId { get; set; }
        public string CallbackUrl { get; set; } = "{0}";
        public DateTime? ClaimedDateTime { get; set; }
        public DateTime? ValidUntilDateTime { get; set; } = DateTime.UtcNow.AddMonths(7);
        public Guid Token { get; set; } = Guid.NewGuid();
        public int UserRole { get; set; }
        public int UserContentAccessLevels { get; set; }
        public Guid? MembershipId { get; set; }
    }
}

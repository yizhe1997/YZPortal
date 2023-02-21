using System.ComponentModel.DataAnnotations;
using YZPortal.Core.Domain.Database.Dealers;
using YZPortal.Core.Domain.Database.EntityTypes.Auditable;

namespace YZPortal.Core.Domain.Database.Memberships
{
    public class MembershipInvite : EmailableEntity
    {
        [Required]
        public string? Name { get; set; }
		[Required]
		public Guid DealerId { get; set; }
		public string CallbackUrl { get; set; } = "{0}";
        public DateTime? ClaimedDateTime { get; set; }
        public DateTime? ValidUntilDateTime { get; set; } = DateTime.UtcNow.AddMonths(7);
        public Guid Token { get; set; } = Guid.NewGuid();
        public int UserRole { get; set; }
        public int UserContentAccessLevels { get; set; }
        // TO DO: might need to remove this since its redundant..? or
        // add membership invite to the membership entity rmbr about principal and dependent
        // https://stackoverflow.com/questions/60076109/one-to-one-to-many-causes-cycles-or-multiple-cascade-paths-error
        [Required]
        public Membership? Membership { get; set; }
    }
}

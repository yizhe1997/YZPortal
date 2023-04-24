using YZPortal.Core.Domain.Database.EntityTypes.Auditable;

namespace YZPortal.Core.Domain.Database.Users
{
    public class UserInvite : EmailableEntity
    {
        public List<UserInviteDealerSelection> UserInviteDealerSelections { get; set; } = new List<UserInviteDealerSelection>();
        public string CallbackUrl { get; set; } = "{0}";
        public DateTime? ClaimedDateTime { get; set; }
        public DateTime? ValidUntilDateTime { get; set; } = DateTime.UtcNow.AddMonths(7);
        public Guid Token { get; set; } = Guid.NewGuid();
    }
}

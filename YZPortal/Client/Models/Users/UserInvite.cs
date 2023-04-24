namespace YZPortal.Client.Models.Users
{
    public class UserInvite
    {
        public string? Email { get; set; }
        public string? CallbackUrl { get; set; }
        public IEnumerable<UserInviteDealerSelection> UserInviteDealerSelections { get; set; } = new List<UserInviteDealerSelection>();
    }

    public class UserInviteDealerSelection
    {
        public Guid DealerId { get; set; }
        public int Role { get; set; }
        public List<int> ContentAccessLevels { get; set; } = new List<int>();
    }
}

namespace YZPortal.Client.Models.Users
{
    public class UserInvite
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? CallbackUrl { get; set; }
        public int Role { get; set; }
        public List<int> ContentAccessLevels { get; set; } = new List<int>();
    }
}

namespace YZPortal.Client.Models.Users
{
    public class UserDetail
    {
        public string? DisplayName { get; set; }
        public string? Email { get; set; }
        public string? AuthToken { get; set; }
        public Guid Id { get; set; }
    }
}

namespace YZPortal.API.Controllers.Memberships.Invites
{
    public class InviteViewModel : AuditableModel
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string CallbackUrl { get; set; } = "{0}";
        public DateTime? Claimed { get; set; }
        public DateTime? ValidUntil { get; set; }
        public DateTime? Sent { get; set; }
        public DateTime? Failed { get; set; }
        public string FailedMessage { get; set; }
        public int Attempts { get; set; }
        public DateTime? LastAttempted { get; set; }
    }
}

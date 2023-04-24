using YZPortal.API.Controllers.ViewModel.Auditable;

namespace YZPortal.API.Controllers.Users.Invites
{
    public class InviteViewModel : EmailableViewModel
    {
        public string? CallbackUrl { get; set; } = "{0}";
        public DateTime? ClaimedDateTime { get; set; }
        public DateTime? ValidUntilDateTime { get; set; }
    }
}

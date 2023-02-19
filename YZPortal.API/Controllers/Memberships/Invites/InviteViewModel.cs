using YZPortal.API.Controllers.ViewModel.Auditable;

namespace YZPortal.API.Controllers.Memberships.Invites
{
    public class InviteViewModel : EmailableViewModel
	{
        public string? Name { get; set; }
        public string CallbackUrl { get; set; } = "{0}";
        public DateTime? ClaimedDateTime { get; set; }
        public DateTime? ValidUntilDateTime { get; set; }
    }
}

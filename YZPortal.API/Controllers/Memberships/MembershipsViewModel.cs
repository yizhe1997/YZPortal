using YZPortal.Api.Controllers.ContentAccessLevels;
using YZPortal.Api.Controllers.DealerRoles;
using YZPortal.API.Controllers.ViewModel.Auditable;

namespace YZPortal.Api.Controllers.Memberships
{
    public class MembershipsViewModel : AuditableViewModel
    {
        public Guid UserId { get; set; }
        // Check if it will map to the nested properties
        public string? UserEmail { get; set; }
        public string? UserName { get; set; }
        public string? DealerName { get; set; }
        public bool Admin { get; set; } = false;
        public List<ContentAccessLevelsViewModel> MembershipContentAccessLevels { get; set; }
        public DealerRolesViewModel MembershipDealerRoles { get; set; }
    }
}

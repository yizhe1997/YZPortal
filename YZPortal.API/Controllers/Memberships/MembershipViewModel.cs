using YZPortal.API.Controllers.Memberships.ContentAccessLevels;
using YZPortal.API.Controllers.Memberships.DealerRoles;
using YZPortal.API.Controllers.ViewModel.Auditable;
using YZPortal.Core.Domain.Database.Memberships;

namespace YZPortal.Api.Controllers.Memberships
{
    public class MembershipViewModel : AuditableViewModel
    {
        public Guid UserId { get; set; }
        public string? UserEmail { get; set; }
        public string? UserName { get; set; }
        public string? DealerName { get; set; }
        public bool Admin { get; set; } = false;
        public List<MembershipContentAccessLevelViewModel>? MembershipContentAccessLevels { get; set; }
        public MembershipDealerRoleViewModel? MembershipDealerRole { get; set; }
    }

    public class MembershipDealerRoleViewModel : AuditableViewModel
    {
        public DealerRoleViewModel? DealerRole { get; set; }
    }

    public class MembershipContentAccessLevelViewModel : AuditableViewModel
    {
        public ContentAccessLevelViewModel? ContentAccessLevel { get; set; }
    }
}

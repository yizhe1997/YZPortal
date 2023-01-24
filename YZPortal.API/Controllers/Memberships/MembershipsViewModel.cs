using YZPortal.Api.Controllers.ContentAccessLevels;
using YZPortal.Api.Controllers.DealerRoles;

namespace YZPortal.Api.Controllers.Memberships
{
    public class MembershipsViewModel : AuditableModel
    {
        public Guid UserId { get; set; }
        // Check if it will map to the nested properties
        public string UserEmail { get; set; }
        public string UserName { get; set; }
        public string DealerName { get; set; }
        public bool Admin { get; set; } = false;
        public List<MembershipContentAccessLevelViewModel> MembershipContentAccessLevels { get; set; }
        public MembershipDealerRoleViewModel MembershipDealerRoles { get; set; }
    }

    public class MembershipsCreateViewModel : AuditableModel
    {
        #region MembershipInvite

        public string Name { get; set; }
        public string CallbackUrl { get; set; }
        public DateTime? ClaimedDateTime { get; set; }
        public DateTime? ValidUntilDateTime { get; set; }

        #endregion

        #region Emailable Entity

        public string Email { get; set; }
        public DateTime? SentDateTime { get; set; }
        public string FailedMessage { get; set; }
        public DateTime? FailedSentDateTime { get; set; }
        public int Attempts { get; set; }
        public DateTime? LastAttemptedSentDateTime { get; set; }

        #endregion
    }

    public class MembershipContentAccessLevelViewModel
    {
        public ContentAccessLevelsViewModel ContentAccessLevel { get; set; }
    }
    public class MembershipDealerRoleViewModel
    {
        public DealerRolesViewModel DealerRole { get; set; }
    }
}

using YZPortal.Core.Domain.Database.EntityTypes.Auditable;

namespace YZPortal.Core.Domain.Database.EntityTypes.Users.Configs
{
    public class PortalConfig : AuditableEntity
    {
        public bool UseTabSet { get; set; } = true;
        public string? Theme { get; set; }
        public bool IsFixedHeader { get; set; } = true;
        public bool IsFixedFooter { get; set; } = true;
        public bool IsFullSide { get; set; } = true;
        public bool ShowFooter { get; set; } = true;
        public string? UserSubjectIdentifier { get; set; }
        public User? User { get; set; }
    }
}

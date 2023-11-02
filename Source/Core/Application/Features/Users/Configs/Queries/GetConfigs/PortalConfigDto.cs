using Application.Models.Abstracts;

namespace Application.Features.Users.Configs.Queries.GetConfigs
{
    public class PortalConfigDto : AuditableModel<Guid>
    {
        public bool UseTabSet { get; set; } = false;

        public string? Theme { get; set; } = "";

        public bool IsFixedHeader { get; set; } = true;

        public bool IsFixedFooter { get; set; } = true;

        public bool IsFullSide { get; set; } = true;

        public bool ShowFooter { get; set; } = true;
    }
}

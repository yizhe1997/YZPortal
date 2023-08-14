namespace YZPortal.FullStackCore.Models.Users.Configs
{
    public class PortalConfigModel
    {
        public bool UseTabSet { get; set; } = false;

        public string? Theme { get; set; } = "";

        public bool IsFixedHeader { get; set; } = true;

        public bool IsFixedFooter { get; set; } = true;

        public bool IsFullSide { get; set; } = true;

        public bool ShowFooter { get; set; } = true;
    }
}

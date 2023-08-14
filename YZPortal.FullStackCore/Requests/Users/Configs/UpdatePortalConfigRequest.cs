namespace YZPortal.FullStackCore.Requests.Users.Configs
{
    public class UpdatePortalConfigRequest
    {
        public bool UseTabSet { get; set; }

        public string? Theme { get; set; }

        public bool IsFixedHeader { get; set; }

        public bool IsFixedFooter { get; set; }

        public bool IsFullSide { get; set; }

        public bool ShowFooter { get; set; }
    }
}

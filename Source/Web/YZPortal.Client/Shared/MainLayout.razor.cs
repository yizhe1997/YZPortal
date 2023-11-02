using Application.Features.Users.Configs.Queries.GetConfigs;
using BootstrapBlazor.Components;

namespace YZPortal.Client.Shared
{
    public sealed partial class MainLayout
    {
        private ConfigsDto Configs { get; set; } = new ConfigsDto();
        private List<MenuItem> Menus { get; set; } = new List<MenuItem>();
        public bool IsRightDrawerOpen { get; set; }
        public string? Action { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            // On refresh try to fetch user configs
            Configs = await LocalStorageService.GetUserConfigs();

            GetMenuItems(Menus);
        }

        // TODO: will upgrade this to fetch the menuitems from the server since language is a thing
        private static void GetMenuItems(List<MenuItem> Menus)
        {
            // Clear menus
            Menus.Clear();

            #region Administration Menu

            var administrationSubMenus = new List<MenuItem>
            {
                //new MenuItem() { Text = "Dealers", Icon = "fa-solid fa-fw fa-table", Url = "/fetchDealers" },
                new MenuItem() { Text = "User Setup", Icon = "fa-solid fa-fw fa-users", Url = "/users" }
            };

            Menus.Add(new MenuItem() { Text = "Administration", Icon = "fa-solid fa-fw fa-users", Items = administrationSubMenus });

            #endregion
        }

        private async Task OnLogout()
        {
            // TODO: Make sure all the infos on log out is removed
            await LocalStorageService.ClearLocalStorage();

            await SignOutManager.SetSignOutState();
            Navigation.NavigateTo("authentication/logout");
        }

        private async Task SetPortalConfig(PortalConfigDto portalConfig)
        {
            Configs.PortalConfig = portalConfig;
            await LocalStorageService.SetUserConfigs(Configs);
        }
    }
}
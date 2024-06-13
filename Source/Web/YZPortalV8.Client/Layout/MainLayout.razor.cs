using Application.Features.Users.Configs.Queries.GetConfigs;
using BootstrapBlazor.Components;
using Application.Extensions;

namespace YZPortal.Client.Layout
{
    public sealed partial class MainLayout
    {
        private string? Username { get; set; } = "";

        public bool IsAuthenticated { get; set; }

        private ConfigsDto Configs { get; set; } = new ConfigsDto()
        {
            PortalConfig = new PortalConfigDto()
            {
                Theme = "color1"
            }
        };

        private List<MenuItem> AdminMenu { get; set; } = [];
        private List<MenuItem> CatalogMenu { get; set; } = [];
        private List<MenuItem> PromotionsMenu { get; set; } = [];
        public bool IsRightDrawerOpen { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            IsAuthenticated = authState.User.Identities?.FirstOrDefault(c => c.IsAuthenticated) is not null;
            Username = authState.User.GetDisplayName() ?? "Anon";

            // On refresh try to fetch user configs
            Configs = await LocalStorageService.GetUserConfigs();

            InitMenu();
        }

        // TODO: will upgrade this to fetch the menuitems from the server since language is a thing
        private void InitMenu()
        {
            // Clear menus

            #region Administration Menu

            AdminMenu.Clear();

            var administrationSubMenus = new List<MenuItem>
            {
                //new MenuItem() { Text = "Dealers", Icon = "fa-solid fa-fw fa-table", Url = "/fetchDealers" },
                new() { Text = "User Setup", Icon = "fa-solid fa-fw fa-users", Url = "/users" },
                new() { Text = "Background Jobs", Icon = "fa-solid fa-fw fa-users", Url = "/backgroundjobs" },
                new() { Text = "Public Chat", Icon = "fa-solid fa-fw fa-users", Url = "/chatroom" }
            };

            AdminMenu.Add(new MenuItem() { Text = "Administration", Icon = "fa-solid fa-fw fa-users", Items = administrationSubMenus });

            #endregion

            #region Catalog Menu

            CatalogMenu.Clear();

            var catalogSubMenus = new List<MenuItem>
            {
                new() { Text = "Products", Icon = "fas fa-folder-tree", Url = "/products" },
                new() { Text = "Categories", Icon = "fas fa-folder-tree", Url = "/#" },
            };

            CatalogMenu.Add(new MenuItem() { Text = "Catalog", Icon = "fas fa-book", Items = catalogSubMenus });

            #endregion

            #region Promotions Menu

            PromotionsMenu.Clear();

            var promotionsSubMenus = new List<MenuItem>
            {
                new() { Text = "Discounts", Icon = "fas fa-hand-holding-dollar", Url = "/#" }
            };

            PromotionsMenu.Add(new MenuItem() { Text = "Promotions", Icon = "fas fa-tag", Items = promotionsSubMenus });

            #endregion
        }

        private async Task SetPortalConfig(PortalConfigDto portalConfig)
        {
            Configs.PortalConfig = portalConfig;
            await LocalStorageService.SetUserConfigs(Configs);
        }
    }
}
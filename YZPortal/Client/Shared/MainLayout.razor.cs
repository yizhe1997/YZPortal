using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components;

namespace YZPortal.Client.Shared
{
    public sealed partial class MainLayout
    {
        private bool UseTabSet { get; set; } = false;

        private string Theme { get; set; } = "";

        private bool IsOpen { get; set; }

        private bool IsFixedHeader { get; set; } = true;

        private bool IsFixedFooter { get; set; } = true;

        private bool IsFullSide { get; set; } = true;

        private bool ShowFooter { get; set; } = true;

        private List<MenuItem> Menus { get; set; } = new List<MenuItem>();
		private string NotAuthorizeUrl { get; set; } = "/Authentication/Login";
        [Inject] 
        NavigationManager Navigation { get; set; }
        [Inject] 
        SignOutSessionStateManager SignOutManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            await GetMenuItems(Menus);
        }

        // will upgrade this to fetch the menuitems from the server since language is a thing
        private async Task GetMenuItems(List<MenuItem> Menus)
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
            await LocalStorageService.ClearLocalStorage();

            //await GetMenuItems(Menus);

            //AuthenticationStateProvider.StateChanged();

            //StateHasChanged();
            await SignOutManager.SetSignOutState();
            Navigation.NavigateTo("authentication/logout");
        }
    }
}
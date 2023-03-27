using Blazored.LocalStorage;

namespace YZPortal.Client.Services.LocalStorage
{
    public static class StartupExtensions
    {
        public static void AddLocalStorageService(this IServiceCollection services)
        {
            services.AddBlazoredLocalStorage();
            services.AddScoped<YZPortal.Client.Services.LocalStorage.ILocalStorageService, LocalStorageService>();

        }
    }
}

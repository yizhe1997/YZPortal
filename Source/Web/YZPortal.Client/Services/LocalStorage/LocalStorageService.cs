using Application.Features.Users.Configs.Queries.GetConfigs;
using Application.Extensions;

namespace YZPortal.Client.Services.LocalStorage
{
    public class LocalStorageProperties
	{
		internal const string UserConfigs = "userConfigs";
	}

	public class LocalStorageService(Blazored.LocalStorage.ILocalStorageService localStorageService) : ILocalStorageService
    {

        #region User

        #region Config

        public async Task SetUserPortalConfig(PortalConfigDto data, CancellationToken cancellationToken = new CancellationToken())
        {
            await localStorageService.SetItemAsync("PortalConfigModel", data, cancellationToken);
        }

        public async Task SetUserConfigs(ConfigsDto data, CancellationToken cancellationToken = new CancellationToken())
        {
            await localStorageService.SetItemAsync(LocalStorageProperties.UserConfigs, data, cancellationToken);
        }

        public async Task RemoveUserConfigs(CancellationToken cancellationToken = new CancellationToken())
		{
			await localStorageService.RemoveItemAsync(LocalStorageProperties.UserConfigs, cancellationToken);
		}

		public async Task<ConfigsDto> GetUserConfigs(CancellationToken cancellationToken = new CancellationToken())
		{
			if (await localStorageService.ContainKeyAsync(LocalStorageProperties.UserConfigs, cancellationToken))
			{
                return await localStorageService.GetItemAsync<ConfigsDto>(LocalStorageProperties.UserConfigs, cancellationToken);
            }

			return new ConfigsDto();
		}

        #endregion

        public async Task<string> GetUserCulture(CancellationToken cancellationToken = new CancellationToken())
        {
            if (await localStorageService.ContainKeyAsync("BlazorCulture", cancellationToken))
            {
                return await localStorageService.GetItemAsync<string>("BlazorCulture", cancellationToken);
            }

            return string.Empty;
        }

        #endregion

        public async Task ClearLocalStorage(CancellationToken cancellationToken = new CancellationToken())
        {
            var localStorageServiceConsts = typeof(LocalStorageProperties).GetConstants();

            foreach (var constant in localStorageServiceConsts)
            {
                await localStorageService.RemoveItemAsync(constant, cancellationToken).ConfigureAwait(false);
            }
        }
    }
}

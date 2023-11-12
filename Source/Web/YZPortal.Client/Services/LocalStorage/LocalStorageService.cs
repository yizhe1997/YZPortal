using Application.Features.Users.Configs.Queries.GetConfigs;
using Application.Extensions;

namespace YZPortal.Client.Services.LocalStorage
{
    public class LocalStorageProperties
	{
		internal const string UserConfigs = "userConfigs";
	}

	public class LocalStorageService : ILocalStorageService
    {
        private readonly Blazored.LocalStorage.ILocalStorageService _localStorageService;

		public LocalStorageService(Blazored.LocalStorage.ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        #region User

        #region Config

        public async Task SetUserPortalConfig(PortalConfigDto data, CancellationToken cancellationToken = new CancellationToken())
        {
            await _localStorageService.SetItemAsync("PortalConfigModel", data, cancellationToken);
        }

        public async Task SetUserConfigs(ConfigsDto data, CancellationToken cancellationToken = new CancellationToken())
        {
            await _localStorageService.SetItemAsync(LocalStorageProperties.UserConfigs, data, cancellationToken);
        }

        public async Task RemoveUserConfigs(CancellationToken cancellationToken = new CancellationToken())
		{
			await _localStorageService.RemoveItemAsync(LocalStorageProperties.UserConfigs, cancellationToken);
		}

		public async Task<ConfigsDto> GetUserConfigs(CancellationToken cancellationToken = new CancellationToken())
		{
			if (await _localStorageService.ContainKeyAsync(LocalStorageProperties.UserConfigs, cancellationToken))
			{
                return await _localStorageService.GetItemAsync<ConfigsDto>(LocalStorageProperties.UserConfigs, cancellationToken);
            }

			return new ConfigsDto();
		}

        #endregion

        public async Task<string> GetUserCulture(CancellationToken cancellationToken = new CancellationToken())
        {
            if (await _localStorageService.ContainKeyAsync("BlazorCulture", cancellationToken))
            {
                return await _localStorageService.GetItemAsync<string>("BlazorCulture", cancellationToken);
            }

            return string.Empty;
        }

        #endregion

        public async Task ClearLocalStorage(CancellationToken cancellationToken = new CancellationToken())
        {
            var localStorageServiceConsts = typeof(LocalStorageProperties).GetConstants();

            foreach (var constant in localStorageServiceConsts)
            {
                await _localStorageService.RemoveItemAsync(constant, cancellationToken).ConfigureAwait(false);
            }
        }
    }
}

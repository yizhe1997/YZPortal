using Application.Features.Users.Configs.Queries.GetConfigs;

namespace YZPortal.Client.Services.LocalStorage
{
    public interface ILocalStorageService
    {
        #region User

        #region Config

        // TODO: use interface for the config for better management
        Task SetUserPortalConfig(PortalConfigDto data, CancellationToken cancellationToken = new CancellationToken());
        Task SetUserConfigs(ConfigsDto data, CancellationToken cancellationToken = new CancellationToken());
        Task<ConfigsDto> GetUserConfigs(CancellationToken cancellationToken = new CancellationToken());
        Task RemoveUserConfigs(CancellationToken cancellationToken = new CancellationToken());
        #endregion

        Task<string> GetUserCulture(CancellationToken cancellationToken = new CancellationToken());

        #endregion

        Task ClearLocalStorage(CancellationToken cancellationToken = new CancellationToken());
    }
}

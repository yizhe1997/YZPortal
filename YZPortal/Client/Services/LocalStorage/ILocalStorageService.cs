using YZPortal.FullStackCore.Models.Users.Configs;

namespace YZPortal.Client.Services.LocalStorage
{
    public interface ILocalStorageService
    {
        #region User

        #region Config

        // TODO: use interface for the config for better management
        Task SetUserConfigs(ConfigsModel data, CancellationToken cancellationToken = new CancellationToken());
        Task<ConfigsModel> GetUserConfigs(CancellationToken cancellationToken = new CancellationToken());
        Task RemoveUserConfigs(CancellationToken cancellationToken = new CancellationToken());
        #endregion

        #endregion

        Task ClearLocalStorage(CancellationToken cancellationToken = new CancellationToken());
    }
}

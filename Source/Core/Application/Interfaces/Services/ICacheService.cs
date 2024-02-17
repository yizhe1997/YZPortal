namespace Application.Interfaces.Services
{
    public interface ICacheService
    {
        #region Get

        T? Get<T>(string key);
        Task<T?> GetAsync<T>(string key, CancellationToken token = default);

        #endregion

        #region Refresh

        void Refresh(string key);
        Task RefreshAsync(string key, CancellationToken token = default);

        #endregion

        #region Remove

        void Remove(string key);
        Task RemoveAsync(string key, CancellationToken token = default);

        #endregion

        #region Set

        void Set<T>(string key, T value);
        Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default);

        #endregion

        #region GetOrSet

        T? GetOrSet<T>(string key, Func<T?> getItemCallback);
        Task<T?> GetOrSetAsync<T>(string key, Func<Task<T>> getItemCallback, CancellationToken cancellationToken = default);

        #endregion
    }
}

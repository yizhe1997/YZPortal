using Application.Interfaces.Services;
using Microsoft.Extensions.Caching.Distributed;
using Application.Extensions;
using Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.Memory;
namespace Infrastructure.Services.Caching
{
    public class DistributedCacheService : ICacheService
    {
        private readonly IDistributedCache _cache;
        private readonly ISerializerService _serializer;
        private readonly DistributedCacheEntryOptions _distributedCacheEntryOptions;
        private readonly CacheConfig _cacheConfig;
        private static readonly SemaphoreSlim semaphore = new(1, 1);

        public DistributedCacheService(IDistributedCache cache, ISerializerService serializer, IOptions<CacheConfig> cacheConfig)
        {
            _cache = cache;
            _serializer = serializer;
            _cacheConfig = cacheConfig.Value;
            _distributedCacheEntryOptions = new DistributedCacheEntryOptions();
            // TODO: add overload for not using defaults
            _distributedCacheEntryOptions.SetSlidingExpiration(TimeSpan.FromSeconds(_cacheConfig.DefaultSlidingExpirationSeconds));
            _distributedCacheEntryOptions.SetAbsoluteExpiration(TimeSpan.FromSeconds(_cacheConfig.DefaultAbsoluteExpirationSeconds));
        }

        #region Get

        public T? Get<T>(string key) => _cache.Get(key) is { } data
            ? _serializer.Deserialize<T>(data.GetString())
            : default;

        public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) =>
            await _cache.GetAsync(key, cancellationToken) is { } data
                ? _serializer.Deserialize<T>(data.GetString())
                : default;

        #endregion

        #region Refresh

        public void Refresh(string key) => _cache.Refresh(key);

        public async Task RefreshAsync(string key, CancellationToken cancellationToken = default) =>
            await _cache.RefreshAsync(key, cancellationToken);

        #endregion

        #region Remove

        public void Remove(string key) => _cache.Remove(key);

        public async Task RemoveAsync(string key, CancellationToken cancellationToken = default) =>
            await _cache.RemoveAsync(key, cancellationToken);

        #endregion

        #region Set

        public void Set<T>(string key, T value) =>
            _cache.Set(key, _serializer.Serialize(value).GetBytes(), _distributedCacheEntryOptions);

        public async Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default) =>
            await _cache.SetAsync(key, _serializer.Serialize(value).GetBytes(), _distributedCacheEntryOptions, cancellationToken);

        #endregion

        #region GetOrSet

        public T? GetOrSet<T>(string key, Func<T?> getItemCallback)
        {
            var value = Get<T>(key);

            if (value is not null)
            {
                return value;
            }

            try
            {
                semaphore.Wait();

                value = Get<T>(key);

                if (value is not null)
                {
                    return value;
                }

                value = getItemCallback();

                if (value is not null)
                {
                    Set(key, value);
                }
            }
            finally
            {
                semaphore.Release();
            }

            return value;
        }

        public async Task<T?> GetOrSetAsync<T>(string key, Func<Task<T>> getItemCallback, CancellationToken cancellationToken = default)
        {
            var value = await GetAsync<T>(key, cancellationToken);

            if (value is not null)
            {
                return value;
            }

            try
            {
                await semaphore.WaitAsync(cancellationToken);

                value = await GetAsync<T>(key, cancellationToken);

                if (value is not null)
                {
                    return value;
                }

                value = await getItemCallback();

                if (value is not null)
                {
                    await SetAsync(key, value, cancellationToken);
                }
            }
            finally
            {
                semaphore.Release();
            }

            return value;
        }

        #endregion
    }
}

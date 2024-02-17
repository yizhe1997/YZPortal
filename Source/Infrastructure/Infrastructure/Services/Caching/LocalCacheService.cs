using Application.Interfaces.Services;
using Infrastructure.Configurations;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services.Caching
{
    public class LocalCacheService : ICacheService
    {
        private readonly IMemoryCache _cache;
        private readonly MemoryCacheEntryOptions _memoryCacheEntryOptions;
        private readonly CacheConfig _cacheConfig;
        private static readonly SemaphoreSlim semaphore = new(1, 1);

        public LocalCacheService(IMemoryCache cache, IOptions<CacheConfig> cacheConfig)
        {
            _cache = cache;
            _cacheConfig = cacheConfig.Value;
            _memoryCacheEntryOptions = new MemoryCacheEntryOptions();
            // TODO: add overload for not using defaults
            _memoryCacheEntryOptions.SetSlidingExpiration(TimeSpan.FromSeconds(_cacheConfig.DefaultSlidingExpirationSeconds));
            _memoryCacheEntryOptions.SetAbsoluteExpiration(TimeSpan.FromSeconds(_cacheConfig.DefaultAbsoluteExpirationSeconds));
            _memoryCacheEntryOptions.SetPriority(CacheItemPriority.Normal);
        }

        #region Get

        public T? Get<T>(string key) => _cache.Get<T>(key);

        public Task<T?> GetAsync<T>(string key, CancellationToken token = default) =>
            Task.FromResult(Get<T>(key));

        #endregion

        #region Refresh

        public void Refresh(string key) =>
            _cache.TryGetValue(key, out _);

        public Task RefreshAsync(string key, CancellationToken token = default)
        {
            Refresh(key);
            return Task.CompletedTask;
        }

        #endregion

        #region Remove

        public void Remove(string key) => _cache.Remove(key);

        public Task RemoveAsync(string key, CancellationToken token = default)
        {
            Remove(key);
            return Task.CompletedTask;
        }

        #endregion

        #region Set

        public void Set<T>(string key, T value) => _cache.Set(key, value, _memoryCacheEntryOptions);

        public Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default)
        {
            Set(key, value);
            return Task.CompletedTask;
        }

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

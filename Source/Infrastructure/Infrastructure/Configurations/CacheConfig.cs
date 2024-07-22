using Domain.Enums;

namespace Infrastructure.Configurations
{
    public class CacheConfig
    {
        public DistributedCacheType DistributedCacheType { get; set; }
        public int DefaultAbsoluteExpirationSeconds { get; set; } = 3600;
        public int DefaultSlidingExpirationSeconds { get; set; } = 60;
        public RedisConfig Redis { get; set; } = new();

        public class RedisConfig
        {
            public string? Url { get; set; }
        }
    }
}

namespace Domain.Enums
{
    [Flags]
    public enum DistributedCacheType
    {
        None, // Local cache service
        InMemory,
        Redis,
        SqlServer
    }
}

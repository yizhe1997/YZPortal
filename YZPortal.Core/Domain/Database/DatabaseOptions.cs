namespace YZPortal.Core.Domain.Database
{
    public class SeedOptions
    {
        public string? AdminEmail { get; set; }
        public string? AdminPassword { get; set; }
        public bool SeedDatabase { get; set; } = true;
    }
}

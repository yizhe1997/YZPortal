namespace YZPortal.API.Infrastructure.Security.AzureAd
{
    public class AzureAdSwaggerOptions
    {
        public string ClientId { get; set; } = "00000000-0000-0000-0000-000000000000";
        public string ClientSecret { get; set; } = "string";
        public string Scope { get; set; } = "api://00000000-0000-0000-0000-000000000000/Authorization";
    }
}

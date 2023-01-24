namespace YZPortal.API.Infrastructure.Security.AzureAd
{
    public class AzureAdApiOptions
    {
        public string ClientId { get; set; } = "00000000-0000-0000-0000-000000000000";
        public string Instance { get; set; } = "https://login.microsoftonline.com";
        public string Domain { get; set; } = "string.net";
        public string TenantId { get; set; } = "00000000-0000-0000-0000-000000000000";
        public string Audience { get { return "api://" + ClientId; } set {; } }
    }
}

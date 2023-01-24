namespace YZPortal.API.Infrastructure.Security.AzureAdB2C
{
    public class AzureAdB2CApiOptions
    {
        public string ClientId { get; set; } = "00000000-0000-0000-0000-000000000000";
        public string Instance { get; set; } = "https://string.b2clogin.com";
        public string SignUpSignInPolicyId { get; set; } = "B2C_1_string";
        public string Domain { get; set; } = "string.onmicrosoft.com";
    }
}

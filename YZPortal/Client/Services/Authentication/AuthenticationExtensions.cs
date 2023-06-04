using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace YZPortal.Client.Services.Authentication
{
    public static class AuthenticationExtensions
    {
        public static string? GetNameidentifierClaim(this AuthenticationState authenticationState) =>
            authenticationState.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        public static string? GetIdentityProviderClaim(this AuthenticationState authenticationState) =>
            authenticationState.User.FindFirst("http://schemas.microsoft.com/identity/claims/identityprovider")?.Value;

        public static string? GetEmailAddressClaim(this AuthenticationState authenticationState) =>
            authenticationState.User.FindFirst(ClaimTypes.Email)?.Value;

        public static string? GetGivenNameClaim(this AuthenticationState authenticationState) =>
            authenticationState.User.FindFirst(ClaimTypes.GivenName)?.Value;

        public static string? GetSurNameClaim(this AuthenticationState authenticationState) =>
            authenticationState.User.FindFirst(ClaimTypes.Surname)?.Value;

        public static string? GetAuthnClassReferenceClaim(this AuthenticationState authenticationState) =>
            authenticationState.User.FindFirst("http://schemas.microsoft.com/claims/authnclassreference")?.Value;

        public static string? GetAuthTimeClaim(this AuthenticationState authenticationState) =>
            authenticationState.User.FindFirst("auth_time")?.Value;

        public static string? GetNameClaim(this AuthenticationState authenticationState) =>
            authenticationState.User.FindFirst(ClaimTypes.Name)?.Value;

        public static string? GetIdpAccessTokenClaim(this AuthenticationState authenticationState) =>
            authenticationState.User.FindFirst("idp_access_token")?.Value;

        public static string? GetOidClaim(this AuthenticationState authenticationState) =>
            authenticationState.User.FindFirst("sub")?.Value;

        public static string?[] GetRoleClaim(this AuthenticationState authenticationState)
        {
            var roleClaims = authenticationState.User.FindAll("role");

            var roleDisplayNames = roleClaims?
                .Select(x => x.Value)
                .Select(ParseRoleClaim)
                .Where(role => !string.IsNullOrEmpty(role))
                .ToArray();

            return roleDisplayNames ?? Array.Empty<string>();
        }

        public class test
        {

            [JsonProperty("displayName")]
            public string DisplayName { get; set; }
        }

        public static string? ParseRoleClaim(this string roleClaim)
        {
            try
            {
                // Remove extra escape characters and backslashes
                string cleanedJsonString = roleClaim.Replace("\\r\\n", "").Replace("\\\"", "\"");
                dynamic objt = JsonConvert.DeserializeObject(cleanedJsonString);
                var result = JsonConvert.DeserializeObject<List<test>>(objt);
                var jObject = JObject.Parse(roleClaim);
                if (jObject.TryGetValue("displayName", out var displayName))
                {
                    return displayName.ToString();
                }
            }
            catch (Exception ex)
            {
                // Ignore parsing errors and return null
            }

            return null;
        }
        public static string? GetOidClaim(this ClaimsPrincipal claimsPrincipal) =>
            claimsPrincipal.FindFirst("sub")?.Value;

        public static string? GetRoleClaim(this ClaimsPrincipal claimsPrincipal) =>
            claimsPrincipal.FindFirst("role")?.Value;
    }
}
    
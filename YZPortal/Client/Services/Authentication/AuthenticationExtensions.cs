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

        public static string? GetOidClaim(this ClaimsPrincipal claimsPrincipal) =>
            claimsPrincipal.FindFirst("sub")?.Value;

        public static string[] GetRoleClaim(this ClaimsPrincipal claimsPrincipal) =>
            claimsPrincipal.FindAll("roles").Select(x => x.Value).ToArray() ?? Array.Empty<string>();
    }
}
    
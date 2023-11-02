using System.Security.Claims;

namespace Application.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        //public static string? GetNameidentifierClaim(this AuthenticationState authenticationState) =>
        //    authenticationState.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        //public static string? GetIdentityProviderClaim(this AuthenticationState authenticationState) =>
        //    authenticationState.User.FindFirst("http://schemas.microsoft.com/identity/claims/identityprovider")?.Value;

        //public static string? GetEmailAddressClaim(this AuthenticationState authenticationState) =>
        //    authenticationState.User.FindFirst(ClaimTypes.Email)?.Value;

        //public static string? GetGivenNameClaim(this AuthenticationState authenticationState) =>
        //    authenticationState.User.FindFirst(ClaimTypes.GivenName)?.Value;

        //public static string? GetSurNameClaim(this AuthenticationState authenticationState) =>
        //    authenticationState.User.FindFirst(ClaimTypes.Surname)?.Value;

        //public static string? GetAuthnClassReferenceClaim(this AuthenticationState authenticationState) =>
        //    authenticationState.User.FindFirst("http://schemas.microsoft.com/claims/authnclassreference")?.Value;

        //public static string? GetAuthTimeClaim(this AuthenticationState authenticationState) =>
        //    authenticationState.User.FindFirst("auth_time")?.Value;

        //public static string? GetNameClaim(this AuthenticationState authenticationState) =>
        //    authenticationState.User.FindFirst(ClaimTypes.Name)?.Value;

        //public static string? GetIdpAccessTokenClaim(this AuthenticationState authenticationState) =>
        //    authenticationState.User.FindFirst("idp_access_token")?.Value;

        public static string? GetEmail(this ClaimsPrincipal claimsPrincipal) =>
            claimsPrincipal.FindFirst(ClaimTypes.Email)?.Value;

        public static string? GetAuthClassRef(this ClaimsPrincipal claimsPrincipal) =>
            claimsPrincipal.FindFirst("acr")?.Value;

        public static string? GetDisplayName(this ClaimsPrincipal claimsPrincipal) =>
            claimsPrincipal.FindFirst("name")?.Value;

        public static string? GetFirstName(this ClaimsPrincipal claimsPrincipal) =>
            claimsPrincipal.FindFirst(ClaimTypes.GivenName)?.Value;

        public static string? GetLastName(this ClaimsPrincipal claimsPrincipal) =>
            claimsPrincipal.FindFirst(ClaimTypes.Surname)?.Value;

        public static string? GetAuthTime(this ClaimsPrincipal claimsPrincipal) =>
            claimsPrincipal.FindFirst("iat")?.Value;

        public static string? GetAuthExpireTime(this ClaimsPrincipal claimsPrincipal) =>
            claimsPrincipal.FindFirst("exp")?.Value;

        public static string? GetIdpAccessToken(this ClaimsPrincipal claimsPrincipal) =>
            claimsPrincipal.FindFirst("idp_access_token")?.Value;

        public static string? GetNameIdentifier(this ClaimsPrincipal claimsPrincipal) =>
            claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        public static string? GetIdentityProvider(this ClaimsPrincipal claimsPrincipal) =>
            claimsPrincipal.FindFirst("http://schemas.microsoft.com/identity/claims/identityprovider")?.Value;

        public static string? GetSubClaim(this ClaimsPrincipal claimsPrincipal) =>
            claimsPrincipal.FindFirst("sub")?.Value;

        public static string[] GetUserIdentities(this ClaimsPrincipal claimsPrincipal) =>
            claimsPrincipal.FindAll("currentUserIdentities").Select(x => x.Value).ToArray() ?? Array.Empty<string>();

        public static string[] GetRoleClaim(this ClaimsPrincipal claimsPrincipal) =>
            claimsPrincipal.FindAll(ClaimTypes.Role).Select(x => x.Value).ToArray() ?? Array.Empty<string>();
    }
}

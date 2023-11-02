using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace YZPortal.Client.Shared
{
    /// <summary>
    /// Base class for UserClaims component.
    /// Retrieves claims present in the ID Token issued by Azure AD.
    /// </summary>
    public class UserClaimsBase : ComponentBase
    {
        [Inject]
        private AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        /// <summary>
        /// Retrieves a user claim for the signed-in user.
        /// </summary>
        /// <returns></returns>
        private async Task<Claim?> GetClaimsPrincipalData(string claimType)
        {
            // Gets an AuthenticationState that describes the current user.
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();

            var user = authState.User;

            // Checks if the user has been authenticated.
            if (user.Identity?.IsAuthenticated ?? false)
            {
                return user.Claims.Where(x => x.Type == claimType).FirstOrDefault();
            }

            return null;
        }

        /// <summary>
        /// Retrieves user claims for the signed-in user.
        /// </summary>
        /// <returns></returns>
        private async Task<IEnumerable<Claim>> GetClaimsPrincipalDatas()
        {
            // Gets an AuthenticationState that describes the current user.
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();

            var user = authState.User;

            // Checks if the user has been authenticated.
            if (user.Identity?.IsAuthenticated ?? false)
            {
                return user.Claims;
            }

            return new List<Claim>();
        }

        private async Task<Claim?> GetClaimUserId() => await GetClaimsPrincipalData("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
    }
}


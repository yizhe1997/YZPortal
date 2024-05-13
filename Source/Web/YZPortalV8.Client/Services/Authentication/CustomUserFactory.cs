using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Security.Claims;
using System.Text.Json.Serialization;

// REF: https://learn.microsoft.com/en-us/aspnet/core/blazor/security/webassembly/azure-active-directory-groups-and-roles?view=aspnetcore-7.0
namespace YZPortalV8.Client.Services.Authentication
{
    public class CustomUserFactory : AccountClaimsPrincipalFactory<CustomUserAccount>
    {
        public CustomUserFactory(IAccessTokenProviderAccessor accessor) : base(accessor)
        {
        }

        public async override ValueTask<ClaimsPrincipal> CreateUserAsync(CustomUserAccount account, RemoteAuthenticationUserOptions options)
        {
            var initialUser = await base.CreateUserAsync(account, options);

            if (initialUser.Identity is not null &&
                initialUser.Identity.IsAuthenticated)
            {
                var userIdentity = initialUser.Identity as ClaimsIdentity;

                if (userIdentity is not null)
                {
                    foreach (var claim in account.Roles)
                    {
                        userIdentity.AddClaim(new Claim("roles", claim));
                    }
                }
            }

            return initialUser;
        }
    }

    public class CustomUserAccount : RemoteUserAccount
    {
        [JsonPropertyName("roles")]
        public string[] Roles { get; set; } = Array.Empty<string>()!;
    }
}

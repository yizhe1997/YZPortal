using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Security.Claims;
using System.Text.Json.Serialization;
using YZPortal.Client.Clients.YZPortalApi;
using System.Text.Json;

namespace YZPortal.Client.Services.Authentication
{
    public class CustomUserFactory : AccountClaimsPrincipalFactory<CustomUserAccount>
    {
        //private readonly YZPortalApiHttpClient _yZPortalApiHttpClient;
        public CustomUserFactory(IAccessTokenProviderAccessor accessor) : base(accessor)
        {
            //_yZPortalApiHttpClient = yZPortalApiHttpClient;
        }

        public async override ValueTask<ClaimsPrincipal> CreateUserAsync(CustomUserAccount account, RemoteAuthenticationUserOptions options)
        {
            ClaimsPrincipal user = await base.CreateUserAsync(account, options);

            //if (user?.Identity?.IsAuthenticated ?? false)
            //{
            //    var userIdentity = (ClaimsIdentity)user.Identity;

            //    userIdentity.AddClaim(new Claim("roles", "test", ClaimValueTypes.String, "Graph"));

            //    //var userId = user.GetOidClaim();

            //    //if (!string.IsNullOrEmpty(userId))
            //    //{
            //    //    var userIdentity = (ClaimsIdentity)user.Identity;
            //    //    var graphGroups = await _yZPortalApiHttpClient.GetGraphGroupsForUser(userId);

            //    //    // Loop through the groups and add them to the identity
            //    //    foreach (var group in graphGroups.Results)
            //    //    {
            //    //        userIdentity.AddClaim(new Claim(ClaimTypes.Role, group.DisplayName, ClaimValueTypes.String, "Graph"));
            //    //    }
            //    //}
            //}

            //if (user?.Identity?.IsAuthenticated ?? false)
            //{
            //    ((ClaimsIdentity)user.Identity).AddClaim(new Claim("office", "VALUE"));

            //    ((ClaimsIdentity)user.Identity).AddClaim(new Claim("roles", "bob"));

            //    if (account.AdditionalProperties.ContainsKey("roles"))
            //    {
            //        var roles = account.AdditionalProperties["roles"] as JsonElement?;

            //        if (roles?.ValueKind == JsonValueKind.Array)
            //        {
            //            foreach (JsonElement element in roles.Value.EnumerateArray())
            //            {
            //                ((ClaimsIdentity)user.Identity).AddClaim(new Claim("roles", element.GetString()));
            //            }
            //        }
            //    }

            //    if (account.AdditionalProperties.ContainsKey("groups"))
            //    {
            //        var roles = account.AdditionalProperties["groups"] as JsonElement?;

            //        if (roles?.ValueKind == JsonValueKind.Array)
            //        {
            //            foreach (JsonElement element in roles.Value.EnumerateArray())
            //            {
            //                ((ClaimsIdentity)user.Identity).AddClaim(new Claim("groups", element.GetString()));
            //            }
            //        }
            //    }
            //}


            return user ?? new ClaimsPrincipal();
        }
    }

    public class CustomUserAccount : RemoteUserAccount
    {
        //[JsonPropertyName("groups")]
        //public string[] Groups { get; set; } = default!;

        //[JsonPropertyName(ClaimTypes.Role)]
        //public string[] Roles { get; set; } = default!;
        [JsonPropertyName("groups")]
        public string[] Groups { get; set; } = default!;

        [JsonPropertyName("roles")]
        public string[] Roles { get; set; } = default!;
    }
}

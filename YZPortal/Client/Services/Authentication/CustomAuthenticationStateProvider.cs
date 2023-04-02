using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using YZPortal.Client.Services.LocalStorage;

namespace YZPortal.Client.Services.Authentication
{
    public class CustomAuthenticationStateProvider : Microsoft.AspNetCore.Components.Authorization.AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorageService;

        public CustomAuthenticationStateProvider(ILocalStorageService tokenService)
        {
            _localStorageService = tokenService;
        }

        public void StateChanged()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var authenToken = await _localStorageService.GetUserAuthenToken();
            var identity = string.IsNullOrEmpty(authenToken) ? 
                new ClaimsIdentity() :
                new ClaimsIdentity(ParseClaimsFromJwt(authenToken), "jwt");
            return new AuthenticationState(new ClaimsPrincipal(identity));
        }

        //public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        //      {
        //          var payload = jwt.Split('.')[1];
        //          var jsonBytes = ParseBase64WithoutPadding(payload);
        //          var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
        //          var claims = keyValuePairs?.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())) ?? new List<Claim>();

        //	return claims;
        //      }

        public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var payloadText = Encoding.UTF8.GetString(jsonBytes);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var payloadObj = JsonSerializer.Deserialize<Dictionary<string, object>>(payloadText, options);

            foreach(var keyValuePair in payloadObj)
            {
                if (keyValuePair.Value is JsonElement keyValueArray && keyValueArray.ValueKind == JsonValueKind.Array)
                {
                    claims.AddRange(keyValueArray.EnumerateArray().Select(jsonElement => new Claim(keyValuePair.Key, jsonElement.GetString())));
                }
                else
                {
                    claims.Add(new Claim(keyValuePair.Key, keyValuePair.Value?.ToString() ?? string.Empty));
                }
            }

            return claims;
        }


        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}

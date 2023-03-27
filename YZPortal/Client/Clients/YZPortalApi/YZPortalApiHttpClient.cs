using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using YZPortal.Client.Models.Dealers;
using YZPortal.Client.Models.Users;
using YZPortal.Client.Services.Authentication;
using YZPortal.Client.Services.LocalStorage;

namespace YZPortal.Client.Clients.YZPortalApi
{
    public class YZPortalApiHttpClient
    {
        private readonly ILogger<YZPortalApiHttpClient> _logger;
        private readonly HttpClient _http;
        private readonly ILocalStorageService _localStorageService;
        private readonly CustomAuthenticationStateProvider _authenticationStateProvider;

        public YZPortalApiHttpClient(ILogger<YZPortalApiHttpClient> logger, HttpClient http, ILocalStorageService tokenService, CustomAuthenticationStateProvider myAuthenticationStateProvider)
        {
            _logger = logger;
            _http = http;
            _localStorageService = tokenService;
            _authenticationStateProvider = myAuthenticationStateProvider;
        }

        #region User

        public async Task<UserLoginResult> UserAuthenticate(UserLogin user)
        {
            using (var response = await _http.PostAsJsonAsync("/api/v1/Authenticate", user, CancellationToken.None))
            {
                try
                {
					if (response.IsSuccessStatusCode)
					{
						var output = await response.Content.ReadFromJsonAsync<UserLoginResult>() ?? new();
						output.IsSuccessStatusCode = true;

						// Store token in cache
						await _localStorageService.SetUserAuthenToken(output);

						// Parse claims from jwt and set user subject claim in local storage
                        if (!string.IsNullOrEmpty(output.AuthToken))
                        {
							var claims = CustomAuthenticationStateProvider.ParseClaimsFromJwt(output.AuthToken);
							var subClaimValue = claims.FirstOrDefault(c => c.Type == "sub")?.Value;
                            var userId = subClaimValue == null ? Guid.Empty : Guid.Parse(subClaimValue);
							await _localStorageService.SetUserId(userId);
						}

						_authenticationStateProvider.StateChanged();

						return output;
					}
				}
				catch (Exception ex)
				{
					_logger.LogError(ex.Message);
				}

                return new UserLoginResult();
            }
        }

        // these should be in another place if not calling runtime.. but for now it's ok cause going to add new details soon
        public async Task<UserDetail> GetUserDetails()
        {
            var userDetail = new UserDetail
            {
                AuthToken = await _localStorageService.GetUserAuthenToken(),
                DisplayName = await _localStorageService.GetUserDisplayName(),
                Email = await _localStorageService.GetUserEmail(),
				Id = await _localStorageService.GetUserId()
			};
            return userDetail;
        }

		public async Task LogoutUser()
		{
			await _localStorageService.RemoveUserAuthenToken().ConfigureAwait(false);
			await _localStorageService.RemoveUserEmail().ConfigureAwait(false);
			await _localStorageService.RemoveUserDisplayName().ConfigureAwait(false);
			await _localStorageService.RemoveUserId().ConfigureAwait(false);

			_authenticationStateProvider.StateChanged();
		}

		#endregion

		#region Dealer

		public async Task<Dealers> GetDealers()
        {
            try
            {
                var requestMsg = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/Dealers");
                requestMsg.Headers.Add("Authorization", "Bearer " + await _localStorageService.GetUserAuthenToken());
                var response = await _http.SendAsync(requestMsg);
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized) // NOTE: THEN TOKEN HAS EXPIRED
                {
                    await _localStorageService.RemoveUserAuthenToken().ConfigureAwait(false);
                }
                else if (response.IsSuccessStatusCode)
                {
                    var dealers = await response.Content.ReadFromJsonAsync<Dealers>() ?? new();

                    return dealers;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return new Dealers();
        }

        #endregion
    }
}
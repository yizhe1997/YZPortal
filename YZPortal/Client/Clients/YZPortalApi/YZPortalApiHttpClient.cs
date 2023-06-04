using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using YZPortal.Client.Models.Dealers;
using YZPortal.Client.Models.Users;
using YZPortal.Client.Services.Authentication;
using YZPortal.Client.Services.LocalStorage;
using YZPortal.FullStackCore.Infrastructure.Security.Authorization;
using YZPortal.FullStackCore.Extensions;
using YZPortal.Client.Models.Abstracts;
using YZPortal.Client.Models.Memberships;
using Microsoft.AspNetCore.Components.Authorization;
using YZPortal.Client.Models.Graph.Groups;
using YZPortal.Client.Models.Graph.Users;

namespace YZPortal.Client.Clients.YZPortalApi
{
	public class YZPortalApiHttpClient
    {
        private readonly ILogger<YZPortalApiHttpClient> _logger;
        private readonly HttpClient _http;
        private readonly ILocalStorageService _localStorageService;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public YZPortalApiHttpClient(ILogger<YZPortalApiHttpClient> logger, HttpClient http, ILocalStorageService tokenService, AuthenticationStateProvider authenticationStateProvider)
        {
            _logger = logger;
            _http = http;
            _localStorageService = tokenService;
            _authenticationStateProvider = authenticationStateProvider;
        }

        #region Helpers

        public async Task<HttpRequestMessage> CreateAuthHttpRequestMessage(string relativeUri, HttpMethod httpMethod)
		{
            // Construct HttpRequestMessage with Uri
            var requestMsg = new HttpRequestMessage(httpMethod, _http.BaseAddress + relativeUri);

			return requestMsg;
		}

		public async Task<HttpRequestMessage> CreatePaginatedAuthHttpRequestMessage(string relativeUri, int pageSize = 10, int pageNumber = 1, string? searchString = null, string? orderBy = null)
        {
			// Construct authenticated/authorized HttpRequestMessage with Uri
			var requestMsg = await CreateAuthHttpRequestMessage(relativeUri, HttpMethod.Get);

			// Add pagination query params to HttpRequestMessage
			requestMsg.AddQueryParam(nameof(PagedModel<User>.PageSize), pageSize.ToString());
			requestMsg.AddQueryParam(nameof(PagedModel<User>.PageNumber), pageNumber.ToString());
            if (!string.IsNullOrEmpty(searchString))
			    requestMsg.AddQueryParam(nameof(PagedModel<User>.SearchString), searchString);
			if (!string.IsNullOrEmpty(orderBy))
				requestMsg.AddQueryParam(nameof(PagedModel<User>.OrderBy), orderBy);

			return requestMsg;
		}

        #region Custom Parameters

        // this feels redundant
        public void AddCustomQueryParam(HttpRequestMessage requestMsg, string key, string value)
        {
            requestMsg.AddQueryParam(key, value);
        }

        #endregion

        #endregion

        #region Graph

        #region Users

        public async Task<GraphUsers> GetGraphUsers(int pageSize = 10, int pageNumber = 1, string? searchString = null, string? orderBy = null)
        {
            var requestMsg = await CreatePaginatedAuthHttpRequestMessage($"api/v1/GraphUsers", pageSize, pageNumber, searchString, orderBy);

            using var response = await _http.SendAsync(requestMsg);
            try
            {
                var output = await response.Content.ReadFromJsonAsync<GraphUsers>() ?? new();

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized) // NOTE: THEN TOKEN HAS EXPIRED
                {
                    await _localStorageService.RemoveUserAuthenToken().ConfigureAwait(false);
                }

                return output;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return new GraphUsers();
        }

        #endregion

        #region Groups

        public async Task<GraphGroups> GetGraphGroupsForUser(string userId, int pageSize = 10, int pageNumber = 1, string? searchString = null, string? orderBy = null)
        {
            var requestMsg = await CreatePaginatedAuthHttpRequestMessage($"api/v1/GraphGroups", pageSize, pageNumber, searchString, orderBy);

            // Query parameters
            if (!string.IsNullOrEmpty(userId))
                requestMsg.AddQueryParam("graphUserId", userId);

            using var response = await _http.SendAsync(requestMsg);
            try
            {
                var output = await response.Content.ReadFromJsonAsync<GraphGroups>() ?? new();

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized) // NOTE: THEN TOKEN HAS EXPIRED
                {
                    await _localStorageService.RemoveUserAuthenToken().ConfigureAwait(false);
                }

                return output;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return new GraphGroups();
        }

        #endregion

        #endregion

        #region Users

        public async Task<UserLoginResult> UserAuthenticate(UserLogin user)
        {
			using var response = await _http.PostAsJsonAsync("/api/v1/Authenticate", user);
			try
			{
                var output = await response.Content.ReadFromJsonAsync<UserLoginResult>() ?? new();

                if (response.IsSuccessStatusCode)
				{
					output.IsStatusCodeSucess = true;

					// Store token in cache
					await _localStorageService.SetUserAuthenToken(output);

					// Parse claims from jwt and set user subject claim in local storage
					if (!string.IsNullOrEmpty(output.AuthToken))
					{
						var claims = CustomAuthenticationStateProvider.ParseClaimsFromJwt(output.AuthToken);
						var subClaimValue = claims.FirstOrDefault(c => c.Type == Claims.UserId)?.Value;
						var userId = subClaimValue == null ? Guid.Empty : Guid.Parse(subClaimValue);
						await _localStorageService.SetUserId(userId);
					}
				}

                return output;
            }
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}

			return new UserLoginResult();
		}

        public async Task<UserDealerAuthorizeResult> UserAuthorize(Guid dealerId)
        {
			var requestMsg = await CreateAuthHttpRequestMessage($"api/v1/Authorize", HttpMethod.Post);

			var json = JsonSerializer.Serialize(new { dealerId = dealerId.ToString() }) ?? "{}";
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            requestMsg.Content = content;

            using var response = await _http.SendAsync(requestMsg);
            try
            {
                var output = await response.Content.ReadFromJsonAsync<UserDealerAuthorizeResult>() ?? new();

                if (response.IsSuccessStatusCode)
                {
                    output.IsStatusCodeSucess = true;

                    // Store token in cache
                    await _localStorageService.SetUserAuthenToken(output);

                    // Parse claims from jwt and obtain user's dealerId, content access levels, dealer roles
                    // to be stored in local storage
                    if (!string.IsNullOrEmpty(output.AuthToken))
                    {
                        var claims = CustomAuthenticationStateProvider.ParseClaimsFromJwt(output.AuthToken);

                        // should create a function for this repetitive code
                        var dealerIdClaimValue = claims.FirstOrDefault(c => c.Type == Claims.UserdealerId)?.Value;
                        var userDealerId = dealerIdClaimValue == null ? Guid.Empty : Guid.Parse(dealerIdClaimValue);
                        await _localStorageService.SetUserDealerId(userDealerId);
                    }
                }

                return output;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return new UserDealerAuthorizeResult();
        }

        // these should be in another place if not calling runtime.. but for now it's ok cause going to add new details soon
        public async Task<UserDetail> GetUserDetails()
        {
            // for runtime... this should be used from local storage service
            var userDetail = new UserDetail
            {
                AuthToken = await _localStorageService.GetUserAuthenToken(),
                DisplayName = await _localStorageService.GetUserDisplayName(),
                Email = await _localStorageService.GetUserEmail(),
				Id = await _localStorageService.GetUserId()
			};
            return userDetail;
        }

		public async Task<Users> GetUsers(int pageSize = 10, int pageNumber = 1, string? searchString = null, string? orderBy = null)
		{
            var requestMsg = await CreatePaginatedAuthHttpRequestMessage($"api/v1/Users", pageSize, pageNumber, searchString, orderBy);

			using var response = await _http.SendAsync(requestMsg);
			try
			{
				var output = await response.Content.ReadFromJsonAsync<Users>() ?? new();

				if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized) // NOTE: THEN TOKEN HAS EXPIRED
				{
					await _localStorageService.RemoveUserAuthenToken().ConfigureAwait(false);
				}

				return output;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}

			return new Users();
		}

        public async Task<UserLoginResult> UserInvite(UserInvite user)
        {
            using var response = await _http.PostAsJsonAsync("/api/v1/Authenticate", user);
            try
            {
                var output = await response.Content.ReadFromJsonAsync<UserLoginResult>() ?? new();

                if (response.IsSuccessStatusCode)
                {
                    output.IsStatusCodeSucess = true;

                    // Store token in cache
                    await _localStorageService.SetUserAuthenToken(output);

                    // Parse claims from jwt and set user subject claim in local storage
                    if (!string.IsNullOrEmpty(output.AuthToken))
                    {
                        var claims = CustomAuthenticationStateProvider.ParseClaimsFromJwt(output.AuthToken);
                        var subClaimValue = claims.FirstOrDefault(c => c.Type == Claims.UserId)?.Value;
                        var userId = subClaimValue == null ? Guid.Empty : Guid.Parse(subClaimValue);
                        await _localStorageService.SetUserId(userId);
                    }
                }

                return output;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return new UserLoginResult();
        }

        public async Task<HttpResponseMessage> DeleteUser(Guid userId)
        {
            var requestMsg = await CreateAuthHttpRequestMessage($"api/v1/users/{userId}", HttpMethod.Delete);
            var response = await _http.SendAsync(requestMsg);
            return response;
        }

        #endregion

        #region Dealers

        public async Task<Dealers> GetDealers(int pageSize = 10, int pageNumber = 1, string? searchString = null, string? orderBy = null)
        {
			var requestMsg = await CreatePaginatedAuthHttpRequestMessage($"api/v1/Dealers", pageSize, pageNumber, searchString, orderBy);

            using var response = await _http.SendAsync(requestMsg);
			try
            {
                var output = await response.Content.ReadFromJsonAsync<Dealers>() ?? new();

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized) // NOTE: THEN TOKEN HAS EXPIRED
                {
                    await _localStorageService.RemoveUserAuthenToken().ConfigureAwait(false);
                }

                return output;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return new Dealers();
        }

        #endregion

        #region Memberships

        #region Content Access Levels

        // TODO: make it such that each dealer has its own set of content access levels
        public async Task<ContentAccessLevels> GetContentAccessLevels(int pageSize = 10, int pageNumber = 1, string? searchString = null, string? orderBy = null)
        {
            var requestMsg = await CreatePaginatedAuthHttpRequestMessage($"api/v1/ContentAccessLevels", pageSize, pageNumber, searchString, orderBy);

            using var response = await _http.SendAsync(requestMsg);
            try
            {
                var output = await response.Content.ReadFromJsonAsync<ContentAccessLevels>() ?? new();

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized) // NOTE: THEN TOKEN HAS EXPIRED
                {
                    await _localStorageService.RemoveUserAuthenToken().ConfigureAwait(false);
                }

                return output;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return new ContentAccessLevels();
        }

        #endregion

        #region Roles

        public async Task<DealerRoles> GetDealerRoles(int pageSize = 10, int pageNumber = 1, string? searchString = null, string? orderBy = null)
        {
            var requestMsg = await CreatePaginatedAuthHttpRequestMessage($"api/v1/DealerRoles", pageSize, pageNumber, searchString, orderBy);

            using var response = await _http.SendAsync(requestMsg);
            try
            {
                var output = await response.Content.ReadFromJsonAsync<DealerRoles>() ?? new();

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized) // NOTE: THEN TOKEN HAS EXPIRED
                {
                    await _localStorageService.RemoveUserAuthenToken().ConfigureAwait(false);
                }

                return output;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return new DealerRoles();
        }

        #endregion

        #endregion
    }
}
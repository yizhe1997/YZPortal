﻿using System.Net.Http.Json;
using YZPortal.Client.Services.LocalStorage;
using YZPortal.FullStackCore.Extensions;
using Microsoft.AspNetCore.Components.Authorization;
using YZPortal.FullStackCore.Models.Abstracts;
using YZPortal.FullStackCore.Models.Users;
using YZPortal.FullStackCore.Models.Graph.Users;
using YZPortal.FullStackCore.Models.Graph.Groups;

namespace YZPortal.Client.Clients.YZPortalApi
{
    public class YZPortalApiHttpClient
    {
        private readonly ILogger<YZPortalApiHttpClient> _logger;
        private readonly HttpClient _http;
        private readonly ILocalStorageService _localStorageService;

        public YZPortalApiHttpClient(ILogger<YZPortalApiHttpClient> logger, HttpClient http, ILocalStorageService tokenService)
        {
            _logger = logger;
            _http = http;
            _localStorageService = tokenService;
        }

        #region Helpers

        public HttpRequestMessage CreateAuthHttpRequestMessage(string relativeUri, HttpMethod httpMethod)
		{
            // Construct HttpRequestMessage with Uri
            var requestMsg = new HttpRequestMessage(httpMethod, _http.BaseAddress + relativeUri);

			return requestMsg;
		}

		public HttpRequestMessage CreatePaginatedAuthHttpRequestMessage(string relativeUri, int pageSize = 10, int pageNumber = 1, string? searchString = null, string[]? orderBy = null)
        {
			// Construct authenticated/authorized HttpRequestMessage with Uri
			var requestMsg = CreateAuthHttpRequestMessage(relativeUri, HttpMethod.Get);

			// Add pagination query params to HttpRequestMessage
			requestMsg.AddQueryParam(nameof(SearchModel<UserModel>.PageSize), pageSize.ToString());
			requestMsg.AddQueryParam(nameof(SearchModel<UserModel>.PageNumber), pageNumber.ToString());
            if (!string.IsNullOrEmpty(searchString))
			    requestMsg.AddQueryParam(nameof(SearchModel<UserModel>.SearchString), searchString);

            if (orderBy != null)
            {
                foreach (var val in orderBy)
                {
                    requestMsg.AddQueryParam(nameof(SearchModel<UserModel>.OrderBy), val);
                }
            }

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

        public async Task<GraphUsersModel> GetGraphUsers(int pageSize = 10, int pageNumber = 1, string? searchString = null, string[]? orderBy = null)
        {
            var requestMsg = CreatePaginatedAuthHttpRequestMessage($"api/v1/GraphUsers", pageSize, pageNumber, searchString, orderBy);

            using var response = await _http.SendAsync(requestMsg);
            try
            {
                var output = await response.Content.ReadFromJsonAsync<GraphUsersModel>() ?? new();

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

            return new GraphUsersModel();
        }

        #endregion

        #region Groups

        public async Task<GraphGroupsModel> GetGraphGroupsForUser(string userId, int pageSize = 10, int pageNumber = 1, string? searchString = null, string[]? orderBy = null)
        {
            var requestMsg = CreatePaginatedAuthHttpRequestMessage($"api/v1/GraphGroups", pageSize, pageNumber, searchString, orderBy);

            // Query parameters
            if (!string.IsNullOrEmpty(userId))
                requestMsg.AddQueryParam("graphUserId", userId);

            using var response = await _http.SendAsync(requestMsg);
            try
            {
                var output = await response.Content.ReadFromJsonAsync<GraphGroupsModel>() ?? new();

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

            return new GraphGroupsModel();
        }

        #endregion

        #endregion

        #region Users

        public async Task<UserModel> UserCreate()
        {
            using var response = await _http.PostAsJsonAsync("/api/v1/Users", new { });
            try
            {
                var output = await response.Content.ReadFromJsonAsync<UserModel>() ?? new();

                return output;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return new UserModel();
        }

        public async Task<UserModel> UserUpdate(string subClaim)
        {
            using var response = await _http.PutAsJsonAsync($"/api/v1/Users/{subClaim}", new { });
            try
            {
                var output = await response.Content.ReadFromJsonAsync<UserModel>() ?? new();

                return output;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return new UserModel();
        }

        public async Task<UserModel> GetUser(string subClaim)
        {
            using var response = await _http.GetAsync($"api/v1/Users/{subClaim}");
            try
            {
                var output = await response.Content.ReadFromJsonAsync<UserModel>() ?? new();

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

            return new UserModel();
        }

        public async Task<UsersSearchModel> GetUsers(int pageSize = 10, int pageNumber = 1, string? searchString = null, string[]? orderBy = null)
		{
            var requestMsg = CreatePaginatedAuthHttpRequestMessage($"api/v1/Users", pageSize, pageNumber, searchString, orderBy);

			using var response = await _http.SendAsync(requestMsg);
			try
			{
				var output = await response.Content.ReadFromJsonAsync<UsersSearchModel>() ?? new();

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

			return new UsersSearchModel();
		}

        public async Task<HttpResponseMessage> DeleteUser(Guid userId)
        {
            var response = await _http.DeleteAsync($"api/v1/Users/{userId}");
            return response;
        }

        #endregion
    }
}
using System.Net.Http.Json;
using YZPortal.Client.Services.LocalStorage;
using YZPortal.FullStackCore.Extensions;
using YZPortal.FullStackCore.Models.Users;
using YZPortal.FullStackCore.Models.Graph.Users;
using YZPortal.FullStackCore.Models.Graph.Groups;
using YZPortal.FullStackCore.Requests.Users;
using YZPortal.FullStackCore.Requests.Indexes;

namespace YZPortal.Client.Clients.YZPortalApi
{
    // TODO: Advanced query
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

        public HttpRequestMessage CreateSearchHttpRequestMessage(string relativeUri, int pageSize = 10, int pageNumber = 1, string? searchString = null, string[]? orderBy = null)
        {
            // Construct HttpRequestMessage with GET Http method
            var requestMsg = CreatePaginationHttpRequestMessage(relativeUri, pageSize, pageNumber);

            // Add search query params to HttpRequestMessage
            if (!string.IsNullOrEmpty(searchString))
                requestMsg.AddQueryParam(nameof(SearchRequest.SearchString), searchString);

            if (orderBy != null)
            {
                foreach (var val in orderBy)
                {
                    requestMsg.AddQueryParam(nameof(SearchRequest.OrderBy), val);
                }
            }

            return requestMsg;
        }

        public HttpRequestMessage CreatePaginationHttpRequestMessage(string relativeUri, int pageSize = 10, int pageNumber = 1)
        {
            // Construct HttpRequestMessage with GET Http method
            var requestMsg = new HttpRequestMessage(HttpMethod.Get, _http.BaseAddress + relativeUri);

            // Add pagination query params to HttpRequestMessage
            requestMsg.AddQueryParam(nameof(PagedRequest.PageSize), pageSize.ToString());
            requestMsg.AddQueryParam(nameof(PagedRequest.PageNumber), pageNumber.ToString());

            return requestMsg;
        }

        #endregion

        #region Graph

        #region Users

        public async Task<GraphUsersModel> GetGraphUsers(int pageSize = 10, int pageNumber = 1, string? searchString = null, string[]? orderBy = null)
        {
            var requestMsg = CreateSearchHttpRequestMessage($"api/v1/GraphUsers", pageSize, pageNumber, searchString, orderBy);

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

        public async Task<GraphGroupsModel> GetGraphGroups(string? userSubId = null, int pageSize = 10, int pageNumber = 1, string? searchString = null, string[]? orderBy = null)
        {
            var requestMsg = CreateSearchHttpRequestMessage($"api/v1/GraphGroups", pageSize, pageNumber, searchString, orderBy);

            if (!string.IsNullOrEmpty(userSubId))
                requestMsg.AddQueryParam("userSubjectId", userSubId);

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

        public async Task GraphGroupAddUsers(string? groupId = null, string[]? userSubIds = null)
        {
            using var response = await _http.PostAsJsonAsync("/api/v1/GraphGroups/AddUser", new
            {
                GroupId = groupId,
                UserSubjectIds = userSubIds ?? Array.Empty<string>()
            });
        }

        public async Task GraphGroupRemoveUser(string? groupId = null, string? userSubId = null)
        {
            using var response = await _http.PostAsJsonAsync("/api/v1/GraphGroups/RemoveUser", new
            {
                GroupId = groupId,
                UserSubjectId = userSubId
            });
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

        public async Task<UserModel> UserUpdate(string? subClaim, UpdateUserRequest? updateUserCommand = null)
        {
            using var response = await _http.PutAsJsonAsync($"/api/v1/Users/{subClaim}", updateUserCommand);
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
            var requestMsg = CreateSearchHttpRequestMessage($"api/v1/Users", pageSize, pageNumber, searchString, orderBy);

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
using Application.Features.Users.Configs.Queries.GetConfigs;
using Application.Models;
using GraphModel = Application.Models.Graph;
using IdentityModel = Application.Models.Identity;
using Application.Requests.Users;
using System.Net.Http.Json;
using Application.Requests.Indexes;
using Application.Extensions;
using Application.Features.Users.Configs.Commands.UpdatePortalConfig;

namespace YZPortal.Client.Clients.YZPortalApi
{
    // TODO: the ? statements can be removed for some?
    // TODO: Advanced query
    public class YZPortalApiHttpClient
    {
        private readonly ILogger<YZPortalApiHttpClient> _logger;
        private readonly HttpClient _http;

        public YZPortalApiHttpClient(ILogger<YZPortalApiHttpClient> logger, HttpClient http)
        {
            _logger = logger;
            _http = http;
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

        public async Task<SearchResult<GraphModel.UserModel>> GetGraphUsers(int pageSize = 10, int pageNumber = 1, string? searchString = null, string[]? orderBy = null, CancellationToken cancellationToken = new CancellationToken())
        {
            var requestMsg = CreateSearchHttpRequestMessage($"api/v1/GraphUsers", pageSize, pageNumber, searchString, orderBy);

            using var response = await _http.SendAsync(requestMsg, cancellationToken);
            try
            {
                var output = await response.Content.ReadFromJsonAsync<SearchResult<GraphModel.UserModel>>(cancellationToken: cancellationToken) ?? new();

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized) // NOTE: THEN TOKEN HAS EXPIRED
                {
                }

                return output;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return new SearchResult<GraphModel.UserModel>();
        }

        #endregion

        #region Groups

        public async Task<SearchResult<GraphModel.GroupModel>> GetGraphGroups(string? userSubId = null, int pageSize = 10, int pageNumber = 1, string? searchString = null, string[]? orderBy = null, CancellationToken cancellationToken = new CancellationToken())
        {
            var requestMsg = CreateSearchHttpRequestMessage($"api/v1/GraphGroups", pageSize, pageNumber, searchString, orderBy);

            if (!string.IsNullOrEmpty(userSubId))
                requestMsg.AddQueryParam("userSubId", userSubId);

            using var response = await _http.SendAsync(requestMsg, cancellationToken);
            try
            {
                var output = await response.Content.ReadFromJsonAsync<SearchResult<GraphModel.GroupModel>>(cancellationToken: cancellationToken) ?? new();

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized) // NOTE: THEN TOKEN HAS EXPIRED
                {
                }

                return output;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return new SearchResult<GraphModel.GroupModel>();
        }

        public async Task GraphGroupAddUsers(string? groupId = null, string[]? userSubIds = null, CancellationToken cancellationToken = new CancellationToken())
        {
            using var response = await _http.PostAsJsonAsync("/api/v1/GraphGroups/AddUser", new
            {
                GroupId = groupId,
                UserSubjectIds = userSubIds ?? Array.Empty<string>()
            },
            cancellationToken);
        }

        public async Task GraphGroupRemoveUser(string? groupId = null, string? userSubId = null, CancellationToken cancellationToken = new CancellationToken())
        {
            using var response = await _http.PostAsJsonAsync("/api/v1/GraphGroups/RemoveUser", new
            {
                GroupId = groupId,
                UserSubjectId = userSubId
            },
            cancellationToken);
        }

        #endregion

        #endregion

        #region Users

        public async Task<Result> CreateUserAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            using var response = await _http.PostAsJsonAsync("/api/v1/Users", new { }, cancellationToken: cancellationToken);
            try
            {
                var output = await response.Content.ReadFromJsonAsync<Result>(cancellationToken: cancellationToken) ?? new();

                return output;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return new Result();
        }

        public async Task<Result> UpdateUserAsync(string? subClaim, UpdateUserRequest? updateUserCommand = null, CancellationToken cancellationToken = new CancellationToken())
        {
            using var response = await _http.PutAsJsonAsync($"/api/v1/Users/{subClaim}", updateUserCommand, cancellationToken: cancellationToken);
            try
            {
                var output = await response.Content.ReadFromJsonAsync<Result>(cancellationToken: cancellationToken) ?? new();

                return output;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return new Result();
        }

        public async Task<Result> UpdateCurrentUserViaHttpContextAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            using var response = await _http.PutAsJsonAsync($"/api/v1/Users/UpdateCurrentUserViaHttpContext", new { }, cancellationToken: cancellationToken);
            try
            {
                var output = await response.Content.ReadFromJsonAsync<Result>(cancellationToken: cancellationToken) ?? new();

                return output;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return new Result();
        }

        public async Task<Result<IdentityModel.UserModel>> GetUserAsync(string subClaim, CancellationToken cancellationToken = new CancellationToken())
        {
            using var response = await _http.GetAsync($"api/v1/Users/{subClaim}", cancellationToken);
            try
            {
                var output = await response.Content.ReadFromJsonAsync<Result<IdentityModel.UserModel>>(cancellationToken: cancellationToken) ?? new();

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized) // NOTE: THEN TOKEN HAS EXPIRED
                {
                }

                return output;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return new Result<IdentityModel.UserModel>();
        }

        public async Task<SearchResult<IdentityModel.UserModel>> GetUsersAsync(int pageSize = 10, int pageNumber = 1, string? searchString = null, string[]? orderBy = null, CancellationToken cancellationToken = new CancellationToken())
        {
            var requestMsg = CreateSearchHttpRequestMessage($"api/v1/Users", pageSize, pageNumber, searchString, orderBy);

            using var response = await _http.SendAsync(requestMsg, cancellationToken);
            try
            {
                var output = await response.Content.ReadFromJsonAsync<SearchResult<IdentityModel.UserModel>>(cancellationToken: cancellationToken) ?? new();

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized) // NOTE: THEN TOKEN HAS EXPIRED
                {
                }

                return output;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return new SearchResult<IdentityModel.UserModel>();
        }

        public async Task<Result> DeleteUserAsync(Guid userId, CancellationToken cancellationToken = new CancellationToken())
        {
            using var response = await _http.DeleteAsync($"api/v1/Users/{userId}", cancellationToken);
            try
            {
                var output = await response.Content.ReadFromJsonAsync<Result>(cancellationToken: cancellationToken) ?? new();

                return output;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return new Result();
        }

        #region Configs

        public async Task<Result<ConfigsDto>> GetConfigsAsync(string subClaim, CancellationToken cancellationToken = new CancellationToken())
        {
            using var response = await _http.GetAsync($"api/v1/Configs/{subClaim}", cancellationToken);
            try
            {
                var output = await response.Content.ReadFromJsonAsync<Result<ConfigsDto>>(cancellationToken: cancellationToken) ?? new();

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized) // NOTE: THEN TOKEN HAS EXPIRED
                {
                }

                return output;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return new Result<ConfigsDto>();
        }

        public async Task<Result<PortalConfigDto>> UpdatePortalConfigAsync(string? subClaim, UpdateUserPortalConfigCommand? updateUserPortalConfigCommand = null, CancellationToken cancellationToken = new CancellationToken())
        {
            using var response = await _http.PutAsJsonAsync($"/api/v1/Configs/portalConfiguration/{subClaim}", updateUserPortalConfigCommand, cancellationToken: cancellationToken);
            try
            {
                var output = await response.Content.ReadFromJsonAsync<Result<PortalConfigDto>>(cancellationToken: cancellationToken) ?? new();

                return output;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return new Result<PortalConfigDto>();
        }

        #endregion

        #endregion
    }
}
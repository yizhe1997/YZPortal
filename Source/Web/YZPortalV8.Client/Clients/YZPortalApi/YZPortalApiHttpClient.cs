using Application.Features.Users.Configs.Queries.GetConfigs;
using Application.Models;
using GraphModel = Application.Models.Graph;
using IdentityModel = Application.Models.Identity;
using Application.Requests.Users;
using System.Net.Http.Json;
using Application.Requests.Indexes;
using Application.Extensions;
using Application.Features.Users.Configs.Commands.UpdatePortalConfig;
using System.Net.Http.Headers;
using YZPortalV8.Client.Services.LocalStorage;
using Application.Features.Products.Queries.GetProducts;
using Application.Features.Products.Queries.GetProductCategories;
using Application.Features.Products.Commands.AddProduct;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components.Forms;
using Application.Features.Users.UserProfileImages.Commands;
using System.Net.Http;
using System.Net.Mime;
using System.Text;

namespace YZPortalV8.Client.Clients.YZPortalApi
{
    // TODO: the ? statements can be removed for some?
    // TODO: Advanced query
    public class YZPortalApiHttpClient
    {
        private readonly ILogger<YZPortalApiHttpClient> _logger;
        private readonly HttpClient _http;
        private readonly ILocalStorageService _localStorageService;
        private readonly IJSRuntime _jSRuntime;

        public YZPortalApiHttpClient(ILogger<YZPortalApiHttpClient> logger, HttpClient http, ILocalStorageService localStorageService, IJSRuntime jSRuntime)
        {
            _logger = logger;
            _http = http;
            _localStorageService = localStorageService;
            _jSRuntime = jSRuntime;
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

        public async Task SetHttpRequestMessageHeadersAsync(CancellationToken cancellationToken = default)
        {
            var preferredLanguage = await _localStorageService.GetUserCulture(cancellationToken);

            if (!string.IsNullOrEmpty(preferredLanguage))
                _http.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(preferredLanguage));
        }

        #endregion

        #region Graph

        #region Users

        public async Task<SearchResult<GraphModel.UserModel>> GetGraphUsers(int pageSize = 10, int pageNumber = 1, string? searchString = null, string[]? orderBy = null, CancellationToken cancellationToken = new CancellationToken())
        {
            await SetHttpRequestMessageHeadersAsync(cancellationToken);

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
            await SetHttpRequestMessageHeadersAsync(cancellationToken);

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
            await SetHttpRequestMessageHeadersAsync(cancellationToken);

            using var response = await _http.PostAsJsonAsync("/api/v1/GraphGroups/AddUser", new
            {
                GroupId = groupId,
                UserSubjectIds = userSubIds ?? Array.Empty<string>()
            },
            cancellationToken);
        }

        public async Task GraphGroupRemoveUser(string? groupId = null, string? userSubId = null, CancellationToken cancellationToken = new CancellationToken())
        {
            await SetHttpRequestMessageHeadersAsync(cancellationToken);

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
            await SetHttpRequestMessageHeadersAsync(cancellationToken);

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

        public async Task<Result> UpdateUserAsync(string? subId, UpdateUserCommand? updateUserCommand = null, CancellationToken cancellationToken = new CancellationToken())
        {
            await SetHttpRequestMessageHeadersAsync(cancellationToken);

            using var response = await _http.PutAsJsonAsync($"/api/v1/Users/{subId}", updateUserCommand, cancellationToken: cancellationToken);
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
            await SetHttpRequestMessageHeadersAsync(cancellationToken);

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

        public async Task<Result<IdentityModel.UserModel>> GetUserAsync(string subId, CancellationToken cancellationToken = new CancellationToken())
        {
            await SetHttpRequestMessageHeadersAsync(cancellationToken);

            using var response = await _http.GetAsync($"api/v1/Users/{subId}", cancellationToken);
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
            await SetHttpRequestMessageHeadersAsync(cancellationToken);

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

        public async Task<Result> DeleteUserAsync(string subId, CancellationToken cancellationToken = new CancellationToken())
        {
            await SetHttpRequestMessageHeadersAsync(cancellationToken);

            using var response = await _http.DeleteAsync($"api/v1/Users/{subId}", cancellationToken);
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

        #region User Profile Image

        public async Task<Result> DeleteUserProfileImageAsync(Guid userId)
        {
            await SetHttpRequestMessageHeadersAsync();

            using var response = await _http.DeleteAsync($"api/v1/UserProfileImages/{userId}");
            try
            {
                var output = await response.Content.ReadFromJsonAsync<Result>() ?? new();

                return output;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return new Result();
        }

        public async Task<Result> UploadUserProfileImageAsync(Guid userId, IBrowserFile image)
        {
            await SetHttpRequestMessageHeadersAsync();

            using var ms = image.OpenReadStream();
            using var content = new MultipartFormDataContent
            {
                { new StreamContent(ms, Convert.ToInt32(image.Size)), "command." + nameof(UploadUserProfileImageCommand.File), image.Name }
            };
            content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");

            //using MultipartFormDataContent multipartContent = new()
            //{
            //    { new StringContent("John", Encoding.UTF8, MediaTypeNames.Text.Plain), "first_name" },
            //    { new StringContent("Doe", Encoding.UTF8, MediaTypeNames.Text.Plain), "last_name" }
            //};

            using var response = await _http.PostAsync($"api/v1/UserProfileImages/{userId}", content);
            try
            {
                var output = await response.Content.ReadFromJsonAsync<Result>() ?? new();

                return output;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return new Result();
        }

        public async Task GetUserProfileImageAsync(Guid userId, CancellationToken cancellationToken = new CancellationToken())
        {
            await SetHttpRequestMessageHeadersAsync(cancellationToken);

            var requestMsg = CreateSearchHttpRequestMessage($"api/v1/UserProfileImages/{userId}");

            using var response = await _http.SendAsync(requestMsg, cancellationToken);
            try
            {
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized) // NOTE: THEN TOKEN HAS EXPIRED
                {
                }

                var bytes = await response.Content.ReadAsByteArrayAsync(cancellationToken);
                var contentType = response.Content.Headers.ContentType?.ToString() ?? "application/octet-stream";
                var contentDisposition = response.Content.Headers.ContentDisposition;
                var fileName = contentDisposition?.FileNameStar ?? contentDisposition?.FileName ?? "downloadedFile";

                await _jSRuntime.InvokeVoidAsync("BlazorDownloadFile", new
                {
                    Content = bytes,
                    FileName = fileName,
                    ContentType = contentType
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        #endregion

        #region Configs

        public async Task<Result<ConfigsDto>> GetConfigsAsync(string userSubId, CancellationToken cancellationToken = new CancellationToken())
        {
            await SetHttpRequestMessageHeadersAsync(cancellationToken);

            using var response = await _http.GetAsync($"api/v1/Configs/{userSubId}", cancellationToken);
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

        public async Task<Result<PortalConfigDto>> UpdatePortalConfigAsync(string? userSubId, UpdateUserPortalConfigCommand? updateUserPortalConfigCommand = null, CancellationToken cancellationToken = new CancellationToken())
        {
            await SetHttpRequestMessageHeadersAsync(cancellationToken);

            using var response = await _http.PutAsJsonAsync($"/api/v1/Configs/portalConfiguration/{userSubId}", updateUserPortalConfigCommand, cancellationToken: cancellationToken);
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

        #region Catalog

        #region Products

        public async Task<SearchResult<ProductDto>> GetProductsAsync(int pageSize = 10, int pageNumber = 1, string? searchString = null, string[]? orderBy = null, CancellationToken cancellationToken = new CancellationToken())
        {
            await SetHttpRequestMessageHeadersAsync(cancellationToken);

            var requestMsg = CreateSearchHttpRequestMessage($"api/v1/Products", pageSize, pageNumber, searchString, orderBy);

            using var response = await _http.SendAsync(requestMsg, cancellationToken);
            try
            {
                var output = await response.Content.ReadFromJsonAsync<SearchResult<ProductDto>>(cancellationToken: cancellationToken) ?? new();

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized) // NOTE: THEN TOKEN HAS EXPIRED
                {
                }

                return output;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return new SearchResult<ProductDto>();
        }

        public async Task<byte[]> GetProductsExcelAsync(int pageSize = 10, int pageNumber = 1, string? searchString = null, string[]? orderBy = null, CancellationToken cancellationToken = new CancellationToken())
        {
            await SetHttpRequestMessageHeadersAsync(cancellationToken);

            var requestMsg = CreateSearchHttpRequestMessage($"api/v1/Products/ExportExcel", pageSize, pageNumber, searchString, orderBy);

            using var response = await _http.SendAsync(requestMsg, cancellationToken);
            try
            {
                var output = await response.Content.ReadAsByteArrayAsync(cancellationToken);

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized) // NOTE: THEN TOKEN HAS EXPIRED
                {
                }

                return output;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return [];
        }

        public async Task<Result<Guid>> CreateProductAsync(AddProductCommand command)
        {
            await SetHttpRequestMessageHeadersAsync();

            using var response = await _http.PostAsJsonAsync("/api/v1/Products", command);

            try
            {
                var output = await response.Content.ReadFromJsonAsync<Result<Guid>>() ?? new();

                return output;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return new Result<Guid>();
        }

        public async Task<Result<Guid>> DeleteProductAsync(Guid id)
        {
            await SetHttpRequestMessageHeadersAsync();

            using var response = await _http.DeleteAsync($"/api/v1/Products/{id}");

            try
            {
                var output = await response.Content.ReadFromJsonAsync<Result<Guid>>() ?? new();

                return output;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return new Result<Guid>();
        }


        #endregion

        #region Catergories

        public async Task<SearchResult<ProductCategoryDto>> GetProductCategoriesAsync(int pageSize = 10, int pageNumber = 1, string? searchString = null, string[]? orderBy = null, CancellationToken cancellationToken = new CancellationToken())
        {
            await SetHttpRequestMessageHeadersAsync(cancellationToken);

            var requestMsg = CreateSearchHttpRequestMessage($"api/v1/ProductCategories", pageSize, pageNumber, searchString, orderBy);

            using var response = await _http.SendAsync(requestMsg, cancellationToken);
            try
            {
                var output = await response.Content.ReadFromJsonAsync<SearchResult<ProductCategoryDto>>(cancellationToken: cancellationToken) ?? new();

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized) // NOTE: THEN TOKEN HAS EXPIRED
                {
                }

                return output;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return new SearchResult<ProductCategoryDto>();
        }

        #endregion

        #endregion

        #region Promotions

        #region Discounts

        #endregion

        #endregion
    }
}
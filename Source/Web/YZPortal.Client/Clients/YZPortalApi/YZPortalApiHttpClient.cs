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
using YZPortal.Client.Services.LocalStorage;
using Application.Features.Products.Queries.GetProducts;
using Application.Features.Products.Queries.GetProductCategories;
using Application.Features.Products.Commands.AddProduct;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components.Forms;
using Application.Features.Users.UserProfileImages.Commands;
using System.Net;
using BootstrapBlazor.Components;
using Application.Interfaces;

namespace YZPortal.Client.Clients.YZPortalApi
{
    // TODO: Advanced query
    public class YZPortalApiHttpClient(HttpClient http, ILocalStorageService localStorageService, IJSRuntime jSRuntime)
    {
        #region Helpers

        public HttpRequestMessage CreateSearchHttpRequestMessage(string relativeUri, SearchRequest searchRequest)
		{
			// Construct HttpRequestMessage with GET Http method
			var requestMsg = CreatePaginationHttpRequestMessage(relativeUri, searchRequest);

			// Add search query params to HttpRequestMessage
			if (!string.IsNullOrEmpty(searchRequest.SearchString))
				requestMsg.AddQueryParam(nameof(SearchRequest.SearchString), searchRequest.SearchString);

			if (searchRequest.OrderBy.Length != 0)
			{
				foreach (var val in searchRequest.OrderBy)
				{
					requestMsg.AddQueryParam(nameof(SearchRequest.OrderBy), val);
				}
			}

			return requestMsg;
		}

		public HttpRequestMessage CreatePaginationHttpRequestMessage(string relativeUri, PagedRequest pagedRequest)
        {
            // Construct HttpRequestMessage with GET Http method
            var requestMsg = new HttpRequestMessage(HttpMethod.Get, http.BaseAddress + relativeUri);

            // Add pagination query params to HttpRequestMessage
            requestMsg.AddQueryParam(nameof(PagedRequest.PageSize), pagedRequest.PageSize.ToString());
            requestMsg.AddQueryParam(nameof(PagedRequest.PageNumber), pagedRequest.PageNumber.ToString());

            return requestMsg;
        }

        public async Task SetHttpRequestMessageHeadersAsync(CancellationToken cancellationToken = default)
        {
            var preferredLanguage = await localStorageService.GetUserCulture(cancellationToken);

            if (!string.IsNullOrEmpty(preferredLanguage))
                http.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(preferredLanguage));
        }

        private class ProblemDetails
        {
            public string? Type { get; set; }
            public string? Title { get; set; }
            public int? Status { get; set; }
            public string? Detail { get; set; }
            public string? Instance { get; set; }
        }
        
        // TODO: use interfaces for HandleResponseAsync overloads
        public static async Task<SearchResult<T>> HandleResponseAsync<T>(SearchRequest searchRequest, HttpResponseMessage response, CancellationToken cancellationToken)
		{
			if (response.IsSuccessStatusCode)
			{
				return await response.Content.ReadFromJsonAsync<SearchResult<T>>(cancellationToken: cancellationToken) ?? 
                    await SearchResult<T>.FailAsync(searchRequest, "Response was null which was not expected");
			}
            else
            {
                string errorDetail = await GetErrorDetail(response, cancellationToken);

                return await SearchResult<T>.FailAsync(searchRequest, errorDetail);
            }
		}

		public static async Task<Result<T>> HandleResponseAsync<T>(HttpResponseMessage response, CancellationToken cancellationToken)
		{
			if (response.IsSuccessStatusCode)
			{
				return await response.Content.ReadFromJsonAsync<Result<T>>(cancellationToken: cancellationToken) ??
					await Result<T>.FailAsync("Response was null which was not expected");
			}
			else
			{
                string errorDetail = await GetErrorDetail(response, cancellationToken);

                return await Result<T>.FailAsync(errorDetail);
            }
		}

		public static async Task<Result> HandleResponseAsync(HttpResponseMessage response, CancellationToken cancellationToken)
		{
			if (response.IsSuccessStatusCode)
			{
				return await response.Content.ReadFromJsonAsync<Result>(cancellationToken: cancellationToken) ??
					await Result.FailAsync("Response was null which was not expected");
			}
			else
			{
                string errorDetail = await GetErrorDetail(response, cancellationToken);

                return await Result.FailAsync(errorDetail);
            }
		}

        private static async Task<string> GetErrorDetail(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            return response.StatusCode switch
            {
                HttpStatusCode.NotFound => "Resource not found",
                HttpStatusCode.Unauthorized => "Unauthorized access",
                HttpStatusCode.Forbidden => "Forbidden access",
                HttpStatusCode.BadRequest => await ExtractDetailFromProblemDetails(response, "Invalid request", cancellationToken),
                HttpStatusCode.InternalServerError => await ExtractDetailFromProblemDetails(response, "Server error", cancellationToken),
                _ => "Request failed"
            };
        }

        private static async Task<string> ExtractDetailFromProblemDetails(HttpResponseMessage response, string defaultError, CancellationToken cancellationToken)
        {
            var jsonPayload = await response.Content.ReadFromJsonAsync<ProblemDetails>(cancellationToken: cancellationToken);
            return jsonPayload?.Detail ?? defaultError;
        }

        public static async Task<T> ExecuteCallGuardedAsync<T>(
			Func<Task<T>> service,
			ToastService toastService,
			string? title = null,
            string? successContent = null,
            string? errorContent = null,
            string? warningContent = null,
			bool hideSuccessToast = false,
            bool hideErrorToast = false,
            bool hideWarningToast = false,
            bool isAutoHide = true) where T : IResult, new()
        {
            try
            {
                var result = await service();

                if (result.Succeeded && !hideSuccessToast)
                {
                    await toastService.Success(title, successContent ?? string.Join("", result.Messages), autoHide: isAutoHide);
                }
                else if (!result.Succeeded && !hideErrorToast)
                {
                    await toastService.Error(title, errorContent ?? string.Join("", result.Errors), autoHide: isAutoHide);
                }

                if (result.Warnings.Count != 0 && !hideWarningToast)
                {
                    await toastService.Warning(title, warningContent ?? string.Join("", result.Warnings), autoHide: isAutoHide);
                }

                return result;
            }
            catch (Exception ex)
            {
                await toastService.Error(title, ex.Message, autoHide: isAutoHide);
            }

            // Return a new instance of T if an exception occurs
            return new T();
        }

		#endregion

		#region Graph

		#region Users

		public async Task<SearchResult<GraphModel.UserModel>> GetGraphUsersAsync(SearchRequest searchRequest, CancellationToken cancellationToken = default)
		{
            await SetHttpRequestMessageHeadersAsync(cancellationToken);

            var requestMsg = CreateSearchHttpRequestMessage($"api/v1/GraphUsers", searchRequest);

            using var response = await http.SendAsync(requestMsg, cancellationToken);

            return await HandleResponseAsync<GraphModel.UserModel>(searchRequest, response, cancellationToken);
        }

        #endregion

        #region Groups

        public async Task<SearchResult<GraphModel.GroupModel>> GetGraphGroupsAsync(SearchRequest searchRequest, string? userSubId = null, CancellationToken cancellationToken = default)
        {
            await SetHttpRequestMessageHeadersAsync(cancellationToken);

            var requestMsg = CreateSearchHttpRequestMessage($"api/v1/GraphGroups", searchRequest);

            if (!string.IsNullOrEmpty(userSubId))
                requestMsg.AddQueryParam("userSubId", userSubId);

            using var response = await http.SendAsync(requestMsg, cancellationToken);

            return await HandleResponseAsync<GraphModel.GroupModel>(searchRequest, response, cancellationToken);
        }

		public async Task<Result> GraphGroupAddUsersAsync(string? groupId, string[]? userSubIds = null, CancellationToken cancellationToken = default)
        {
            await SetHttpRequestMessageHeadersAsync(cancellationToken);

            using var response = await http.PostAsJsonAsync("/api/v1/GraphGroups/AddUser", new
            {
                GroupId = groupId,
                UserSubjectIds = userSubIds ?? []
            },
            cancellationToken);

            return await HandleResponseAsync(response, cancellationToken);
        }

		public async Task<Result> GraphGroupRemoveUserAsync(string? groupId, string? userSubId, CancellationToken cancellationToken = default)
        {
            await SetHttpRequestMessageHeadersAsync(cancellationToken);

            using var response = await http.PostAsJsonAsync("/api/v1/GraphGroups/RemoveUser", new
            {
                GroupId = groupId,
                UserSubjectId = userSubId
            },
            cancellationToken);

            return await HandleResponseAsync(response, cancellationToken);
        }

        #endregion

        #endregion

        #region Users

        public async Task<Result> CreateUserAsync(CancellationToken cancellationToken = default)
        {
            await SetHttpRequestMessageHeadersAsync(cancellationToken);

            using var response = await http.PostAsJsonAsync("/api/v1/Users", new { }, cancellationToken: cancellationToken);

            return await HandleResponseAsync(response, cancellationToken);
        }

        public async Task<Result> UpdateUserAsync(string? subId, UpdateUserCommand? updateUserCommand = null, CancellationToken cancellationToken = default)
        {
            await SetHttpRequestMessageHeadersAsync(cancellationToken);

            using var response = await http.PutAsJsonAsync($"/api/v1/Users/{subId}", updateUserCommand, cancellationToken: cancellationToken);

            return await HandleResponseAsync(response, cancellationToken);
        }

        public async Task<Result> UpdateCurrentUserViaHttpContextAsync(CancellationToken cancellationToken = default)
        {
            await SetHttpRequestMessageHeadersAsync(cancellationToken);

            using var response = await http.PutAsJsonAsync($"/api/v1/Users/UpdateCurrentUserViaHttpContext", new { }, cancellationToken: cancellationToken);

            return await HandleResponseAsync(response, cancellationToken);
        }

        public async Task<Result<IdentityModel.UserModel>> GetUserAsync(string? subId, CancellationToken cancellationToken = default)
        {

            await SetHttpRequestMessageHeadersAsync(cancellationToken);

            using var response = await http.GetAsync($"api/v1/Users/{subId}", cancellationToken);

            return await HandleResponseAsync<IdentityModel.UserModel>(response, cancellationToken);
        }

        public async Task<SearchResult<IdentityModel.UserModel>> GetUsersAsync(SearchRequest searchRequest, CancellationToken cancellationToken = default)
        {
            await SetHttpRequestMessageHeadersAsync(cancellationToken);

            var requestMsg = CreateSearchHttpRequestMessage($"api/v1/Users", searchRequest);

            using var response = await http.SendAsync(requestMsg, cancellationToken);

            return await HandleResponseAsync<IdentityModel.UserModel>(searchRequest, response, cancellationToken);
        }

        public async Task<Result> DeleteUserAsync(string? subId, CancellationToken cancellationToken = default)
        {
            await SetHttpRequestMessageHeadersAsync(cancellationToken);

            using var response = await http.DeleteAsync($"api/v1/Users/{subId}", cancellationToken);

            return await HandleResponseAsync(response, cancellationToken);
        }

        #region User Profile Image

        public async Task<Result> DeleteUserProfileImageAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            await SetHttpRequestMessageHeadersAsync(cancellationToken);

            using var response = await http.DeleteAsync($"api/v1/UserProfileImages/{userId}", cancellationToken);

            return await HandleResponseAsync(response, cancellationToken);
        }

        public async Task<Result> UploadUserProfileImageAsync(Guid userId, IBrowserFile image, CancellationToken cancellationToken = default)
        {
            await SetHttpRequestMessageHeadersAsync(cancellationToken);

            using var ms = image.OpenReadStream(cancellationToken: cancellationToken);
            var streamContent = new StreamContent(ms, Convert.ToInt32(image.Size));
            streamContent.Headers.ContentType = new MediaTypeHeaderValue(image.ContentType);

            using var content = new MultipartFormDataContent
                {
                    { streamContent, "command." + nameof(UploadUserProfileImageCommand.File), image.Name }
                };
            content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");

            using var response = await http.PostAsync($"api/v1/UserProfileImages/{userId}", content, cancellationToken);

            return await HandleResponseAsync(response, cancellationToken);
        }

        public async Task<Result> GetUserProfileImageAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            await SetHttpRequestMessageHeadersAsync(cancellationToken);

            using var response = await http.GetAsync($"api/v1/UserProfileImages/{userId}", cancellationToken);

			if (response.IsSuccessStatusCode)
			{
				var bytes = await response.Content.ReadAsByteArrayAsync(cancellationToken);
				var contentType = response.Content.Headers.ContentType?.ToString() ?? "application/octet-stream";
				var contentDisposition = response.Content.Headers.ContentDisposition;
				var fileName = contentDisposition?.FileNameStar ?? contentDisposition?.FileName ?? "downloadedFile";

				await jSRuntime.InvokeVoidAsync("BlazorDownloadFile", new
				{
					Content = bytes,
					FileName = fileName,
					ContentType = contentType
				});

				return await Result.SuccessAsync();
			}
			else
			{
				string errorDetail = await GetErrorDetail(response, cancellationToken);

				return await Result.FailAsync(errorDetail);
			}
		}

        #endregion

        #region Configs

        public async Task<Result<ConfigsDto>> GetUserConfigsAsync(string userSubId, CancellationToken cancellationToken = default)
        {
            await SetHttpRequestMessageHeadersAsync(cancellationToken);

            using var response = await http.GetAsync($"api/v1/Configs/{userSubId}", cancellationToken);

            return await HandleResponseAsync<ConfigsDto>(response, cancellationToken);
        }

        public async Task<Result<PortalConfigDto>> UpdateUserPortalConfigAsync(string? userSubId, UpdateUserPortalConfigCommand? updateUserPortalConfigCommand = null, CancellationToken cancellationToken = default)
        {
            await SetHttpRequestMessageHeadersAsync(cancellationToken);

            using var response = await http.PutAsJsonAsync($"/api/v1/Configs/portalConfiguration/{userSubId}", updateUserPortalConfigCommand, cancellationToken: cancellationToken);

            return await HandleResponseAsync<PortalConfigDto>(response, cancellationToken);
        }

        #endregion

        #endregion

        #region Catalog

        #region Products

        public async Task<SearchResult<ProductDto>> GetProductsAsync(SearchRequest searchRequest, CancellationToken cancellationToken = default)
        {
            await SetHttpRequestMessageHeadersAsync(cancellationToken);

            var requestMsg = CreateSearchHttpRequestMessage($"api/v1/Products", searchRequest);

            using var response = await http.SendAsync(requestMsg, cancellationToken);

            return await HandleResponseAsync<ProductDto>(searchRequest, response, cancellationToken);
        }

		public async Task<Result<byte[]>> GetProductsExcelAsync(SearchRequest searchRequest, CancellationToken cancellationToken = default)
        {
            await SetHttpRequestMessageHeadersAsync(cancellationToken);

            var requestMsg = CreateSearchHttpRequestMessage($"api/v1/Products/ExportExcel", searchRequest);

            using var response = await http.SendAsync(requestMsg, cancellationToken);

			if (response.IsSuccessStatusCode)
            {
                var output = await response.Content.ReadAsByteArrayAsync(cancellationToken) ?? [];

                return await Result<byte[]>.SuccessAsync(output);
            }
            else
            {
                string errorDetail = await GetErrorDetail(response, cancellationToken);

                return await Result<byte[]>.FailAsync(errorDetail);
            }
        }

        public async Task<Result<Guid>> CreateProductAsync(AddProductCommand command, CancellationToken cancellationToken = default)
        {
            await SetHttpRequestMessageHeadersAsync(cancellationToken);

            using var response = await http.PostAsJsonAsync("/api/v1/Products", command, cancellationToken: cancellationToken);

            return await HandleResponseAsync<Guid>(response, cancellationToken);
        }

        public async Task<Result<Guid>> DeleteProductAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await SetHttpRequestMessageHeadersAsync(cancellationToken);

            using var response = await http.DeleteAsync($"/api/v1/Products/{id}", cancellationToken);

            return await HandleResponseAsync<Guid>(response, cancellationToken);
        }

        #endregion

        #region Catergories

        public async Task<SearchResult<ProductCategoryDto>> GetProductCategoriesAsync(SearchRequest searchRequest, CancellationToken cancellationToken = default)
        {
            await SetHttpRequestMessageHeadersAsync(cancellationToken);

            var requestMsg = CreateSearchHttpRequestMessage($"api/v1/ProductCategories", searchRequest);

            using var response = await http.SendAsync(requestMsg, cancellationToken);

            return await HandleResponseAsync<ProductCategoryDto>(searchRequest, response, cancellationToken);
        }

		#endregion

		#endregion

		#region Promotions

		#region Discounts

		#endregion

		#endregion
	}
}
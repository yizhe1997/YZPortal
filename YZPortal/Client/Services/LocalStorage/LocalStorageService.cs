using System.Security.Claims;
using YZPortal.Client.Models.Users;

namespace YZPortal.Client.Services.LocalStorage
{
    public class LocalStorageService : ILocalStorageService
    {
        private readonly Blazored.LocalStorage.ILocalStorageService _localStorageService;

        #region Local Storage Properties

        internal const string UserAuthToken = "authToken";
        internal const string UserId = "sub";
        internal const string UserDisplayName = "displayName";
        internal const string UserEmail = "email";
        internal const string UserdealerId = "dealerId";

        #endregion

        public LocalStorageService(Blazored.LocalStorage.ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        #region User

        #region Authentication

        public async Task SetUserAuthenToken(UserLoginResult userLoginResult)
        {
            await _localStorageService.SetItemAsync(UserAuthToken, userLoginResult.AuthToken);
        }
		public async Task RemoveUserAuthenToken()
		{
			await _localStorageService.RemoveItemAsync(UserAuthToken);
		}

		public async Task<string> GetUserAuthenToken()
		{
			return await _localStorageService.GetItemAsync<string>(UserAuthToken);
		}

        #endregion

        #region General Info

        #region Id

        public async Task SetUserId(Guid Id)
		{
			await _localStorageService.SetItemAsync(UserId, Id);
		}

		public async Task<Guid> GetUserId()
		{
			var stringUserId = await _localStorageService.GetItemAsync<string>(UserId);
			return stringUserId == null ? Guid.Empty : Guid.Parse(stringUserId);
		}

		public async Task RemoveUserId()
		{
			await _localStorageService.RemoveItemAsync(UserId);
		}

		#endregion

		#region DisplayName

		public async Task SetUserDisplayName(string DisplayName)
		{
			await _localStorageService.SetItemAsync(UserDisplayName, DisplayName);
		}

		public async Task<string> GetUserDisplayName()
		{
			return await _localStorageService.GetItemAsync<string>(UserDisplayName);
		}

		public async Task RemoveUserDisplayName()
		{
			await _localStorageService.RemoveItemAsync(UserDisplayName);
		}

		#endregion

		#region Email

		public async Task SetUserEmail(string email)
		{
			await _localStorageService.SetItemAsync(UserEmail, email);
		}

		public async Task<string> GetUserEmail()
		{
			return await _localStorageService.GetItemAsync<string>(UserEmail);
		}

		public async Task RemoveUserEmail()
		{
			await _localStorageService.RemoveItemAsync(UserEmail);
		}

        #endregion

        #region DealerId

        public async Task SetUserDealerId(Guid dealerId)
        {
            await _localStorageService.SetItemAsync(UserdealerId, dealerId);
        }

        public async Task<Guid> GetUserDealerId()
        {
            var stringUserId = await _localStorageService.GetItemAsync<string>(UserdealerId);
            return stringUserId == null ? Guid.Empty : Guid.Parse(stringUserId);
        }

        public async Task RemoveUserDealerId()
        {
            await _localStorageService.RemoveItemAsync(UserdealerId);
        }

        #endregion

        #endregion

        #endregion
    }
}

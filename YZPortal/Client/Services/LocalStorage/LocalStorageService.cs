using YZPortal.Client.Models.Users;

namespace YZPortal.Client.Services.LocalStorage
{
    public class LocalStorageService : ILocalStorageService
    {
        private readonly Blazored.LocalStorage.ILocalStorageService _localStorageService;

        #region Local Storage Properties

        internal const string userAuthToken = "authToken";
        internal const string userId = "sub";
        internal const string userDisplayName = "displayName";
        internal const string userEmail = "email";
        internal const string userdealerId = "dealerId";

        #endregion

        public LocalStorageService(Blazored.LocalStorage.ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        #region User

        #region Authentication

        public async Task SetUserAuthenToken(UserLoginResult userLoginResult)
        {
            await _localStorageService.SetItemAsync(userAuthToken, userLoginResult.AuthToken);
        }
		public async Task RemoveUserAuthenToken()
		{
			await _localStorageService.RemoveItemAsync(userAuthToken);
		}

		public async Task<string> GetUserAuthenToken()
		{
			return await _localStorageService.GetItemAsync<string>(userAuthToken);
		}

		#endregion

		#region Id

		public async Task SetUserId(Guid Id)
		{
			await _localStorageService.SetItemAsync(userId, Id);
		}

		public async Task<Guid> GetUserId()
		{
			var stringUserId = await _localStorageService.GetItemAsync<string>(userId);
			return stringUserId == null ? Guid.Empty : Guid.Parse(stringUserId);
		}

		public async Task RemoveUserId()
		{
			await _localStorageService.RemoveItemAsync(userId);
		}

		#endregion

		#region DisplayName

		public async Task SetUserDisplayName(string DisplayName)
		{
			await _localStorageService.SetItemAsync(userDisplayName, DisplayName);
		}

		public async Task<string> GetUserDisplayName()
		{
			return await _localStorageService.GetItemAsync<string>(userDisplayName);
		}

		public async Task RemoveUserDisplayName()
		{
			await _localStorageService.RemoveItemAsync(userDisplayName);
		}

		#endregion

		#region Email

		public async Task SetUserEmail(string email)
		{
			await _localStorageService.SetItemAsync(userEmail, email);
		}

		public async Task<string> GetUserEmail()
		{
			return await _localStorageService.GetItemAsync<string>(userEmail);
		}

		public async Task RemoveUserEmail()
		{
			await _localStorageService.RemoveItemAsync(userEmail);
		}

        #endregion

        #region DealerId

        public async Task SetUserDealerId(Guid dealerId)
        {
            await _localStorageService.SetItemAsync(userdealerId, dealerId);
        }

        public async Task<Guid> GetUserDealerId()
        {
            var stringUserId = await _localStorageService.GetItemAsync<string>(userdealerId);
            return stringUserId == null ? Guid.Empty : Guid.Parse(stringUserId);
        }

        public async Task RemoveUserDealerId()
        {
            await _localStorageService.RemoveItemAsync(userdealerId);
        }

        #endregion

        #endregion
    }
}

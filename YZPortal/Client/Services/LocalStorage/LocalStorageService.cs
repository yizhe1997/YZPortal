using YZPortal.Client.Models.Users;

namespace YZPortal.Client.Services.LocalStorage
{
    public class LocalStorageService : ILocalStorageService
    {
        private readonly Blazored.LocalStorage.ILocalStorageService _localStorageService;

        public LocalStorageService(Blazored.LocalStorage.ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        #region User

        #region Authentication

        public async Task SetUserAuthenToken(UserLoginResult userLoginResult)
        {
            await _localStorageService.SetItemAsync("userAuthToken", userLoginResult.AuthToken);
        }
		public async Task RemoveUserAuthenToken()
		{
			await _localStorageService.RemoveItemAsync("userAuthToken");
		}

		public async Task<string> GetUserAuthenToken()
		{
			return await _localStorageService.GetItemAsync<string>("userAuthToken");
		}

		#endregion

		#region Id

		public async Task SetUserId(Guid userId)
		{
			await _localStorageService.SetItemAsync("userId", userId);
		}

		public async Task<Guid> GetUserId()
		{
			var stringUserId = await _localStorageService.GetItemAsync<string>("userId");
			return stringUserId == null ? Guid.Empty : Guid.Parse(stringUserId);
		}

		public async Task RemoveUserId()
		{
			await _localStorageService.RemoveItemAsync("userId");
		}

		#endregion

		#region DisplayName

		public async Task SetUserDisplayName(string userDisplayName)
		{
			await _localStorageService.SetItemAsync("userDisplayName", userDisplayName);
		}

		public async Task<string> GetUserDisplayName()
		{
			return await _localStorageService.GetItemAsync<string>("userDisplayName");
		}

		public async Task RemoveUserDisplayName()
		{
			await _localStorageService.RemoveItemAsync("userDisplayName");
		}

		#endregion

		#region Email

		public async Task SetUserEmail(string userEmail)
		{
			await _localStorageService.SetItemAsync("userEmail", userEmail);
		}

		public async Task<string> GetUserEmail()
		{
			return await _localStorageService.GetItemAsync<string>("userEmail");
		}

		public async Task RemoveUserEmail()
		{
			await _localStorageService.RemoveItemAsync("userEmail");
		}

		#endregion

		#endregion
	}
}

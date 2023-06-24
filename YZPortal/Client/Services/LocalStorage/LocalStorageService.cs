using YZPortal.FullStackCore.Extensions;

namespace YZPortal.Client.Services.LocalStorage
{
    public class LocalStorageProperties
	{
		internal const string UserAuthToken = "authToken";
		internal const string UserId = "sub";
		internal const string UserDisplayName = "displayName";
		internal const string UserEmail = "email";
		internal const string UserdealerId = "dealerId";
	}

	public class LocalStorageService : ILocalStorageService
    {
        private readonly Blazored.LocalStorage.ILocalStorageService _localStorageService;

		public LocalStorageService(Blazored.LocalStorage.ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        #region User

        #region Authentication

		public async Task RemoveUserAuthenToken()
		{
			await _localStorageService.RemoveItemAsync(LocalStorageProperties.UserAuthToken);
		}

		public async Task<string> GetUserAuthenToken()
		{
			return await _localStorageService.GetItemAsync<string>(LocalStorageProperties.UserAuthToken);
		}

		public async Task ClearLocalStorage()
		{
			var localStorageServiceConsts = typeof(LocalStorageProperties).GetConstants();

			foreach(var constant in localStorageServiceConsts)
			{
				await _localStorageService.RemoveItemAsync(constant).ConfigureAwait(false);
			}
		}

		#endregion

		#region General Info

		#region Id

		public async Task SetUserId(Guid Id)
		{
			await _localStorageService.SetItemAsync(LocalStorageProperties.UserId, Id);
		}

		public async Task<Guid> GetUserId()
		{
			var stringUserId = await _localStorageService.GetItemAsync<string>(LocalStorageProperties.UserId);
			return stringUserId == null ? Guid.Empty : Guid.Parse(stringUserId);
		}

		public async Task RemoveUserId()
		{
			await _localStorageService.RemoveItemAsync(LocalStorageProperties.UserId);
		}

		#endregion

		#region DisplayName

		public async Task SetUserDisplayName(string DisplayName)
		{
			await _localStorageService.SetItemAsync(LocalStorageProperties.UserDisplayName, DisplayName);
		}

		public async Task<string> GetUserDisplayName()
		{
			return await _localStorageService.GetItemAsync<string>(LocalStorageProperties.UserDisplayName);
		}

		public async Task RemoveUserDisplayName()
		{
			await _localStorageService.RemoveItemAsync(LocalStorageProperties.UserDisplayName);
		}

		#endregion

		#region Email

		public async Task SetUserEmail(string email)
		{
			await _localStorageService.SetItemAsync(LocalStorageProperties.UserEmail, email);
		}

		public async Task<string> GetUserEmail()
		{
			return await _localStorageService.GetItemAsync<string>(LocalStorageProperties.UserEmail);
		}

		public async Task RemoveUserEmail()
		{
			await _localStorageService.RemoveItemAsync(LocalStorageProperties.UserEmail);
		}

        #endregion

        #region DealerId

        public async Task SetUserDealerId(Guid dealerId)
        {
            await _localStorageService.SetItemAsync(LocalStorageProperties.UserdealerId, dealerId);
        }

        public async Task<Guid> GetUserDealerId()
        {
            var stringUserId = await _localStorageService.GetItemAsync<string>(LocalStorageProperties.UserdealerId);
            return stringUserId == null ? Guid.Empty : Guid.Parse(stringUserId);
        }

        public async Task RemoveUserDealerId()
        {
            await _localStorageService.RemoveItemAsync(LocalStorageProperties.UserdealerId);
        }

        #endregion

        #endregion

        #endregion
    }
}

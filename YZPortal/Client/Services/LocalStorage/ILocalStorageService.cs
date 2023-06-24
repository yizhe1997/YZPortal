namespace YZPortal.Client.Services.LocalStorage
{
    public interface ILocalStorageService
    {
        #region User

        #region Authentication

        Task<string> GetUserAuthenToken();
        Task RemoveUserAuthenToken();
		Task ClearLocalStorage();

		#endregion

		#region General Info

		#region Id

		Task<Guid> GetUserId();
		Task RemoveUserId();
		Task SetUserId(Guid Id);

		#endregion

		#region DisplayName

		Task<string> GetUserDisplayName();
        Task RemoveUserDisplayName();
        Task SetUserDisplayName(string DisplayName);

        #endregion

        #region Email

        Task<string> GetUserEmail();
        Task RemoveUserEmail();
        Task SetUserEmail(string email);

        #endregion

        #region DealerId

        Task<Guid> GetUserDealerId();
        Task RemoveUserDealerId();
        Task SetUserDealerId(Guid dealerId);

        #endregion

        #endregion

        #endregion
    }
}

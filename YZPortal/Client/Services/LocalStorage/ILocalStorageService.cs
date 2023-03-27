using YZPortal.Client.Models.Users;

namespace YZPortal.Client.Services.LocalStorage
{
    public interface ILocalStorageService
    {
        #region User

        #region Authentication

        Task<string> GetUserAuthenToken();
        Task RemoveUserAuthenToken();
        Task SetUserAuthenToken(UserLoginResult userLoginResult);

		#endregion

		#region Id

		Task<Guid> GetUserId();
		Task RemoveUserId();
		Task SetUserId(Guid userId);

		#endregion

		#region DisplayName

		Task<string> GetUserDisplayName();
        Task RemoveUserDisplayName();
        Task SetUserDisplayName(string userDisplayName);

        #endregion

        #region Email

        Task<string> GetUserEmail();
        Task RemoveUserEmail();
        Task SetUserEmail(string userEmail);

        #endregion

        #endregion
    }
}

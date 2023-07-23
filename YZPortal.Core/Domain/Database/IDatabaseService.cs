using YZPortal.Core.Domain.Contexts;
using YZPortal.Core.Domain.Database.Users;
using YZPortal.Core.Indexes;

namespace YZPortal.Core.Domain.Database
{
    public interface IDatabaseService
    {
        #region Startup services

        void UserAdmin();
        void EnumValues();
        void SyncStatuses();

        #endregion

        #region User

        Task<SearchList<User>> UsersToSearchListAsync(ISearchParams request, System.Linq.Expressions.Expression<Func<User, bool>>? searchPredicate = null, CancellationToken cancellationToken = default);
        Task<User> UserGetBySubIdAsync(string? subId, CancellationToken cancellationToken = default);
        Task<User> UserGetAsync(Guid Id, CancellationToken cancellationToken = default);
        Task<User> UserCreateAsync<T>(T body, CancellationToken cancellationToken = default) where T : class;
        Task<User> UserUpdateAsync<T>(string? subId, T body, CancellationToken cancellationToken = default) where T : CurrentContext;
        Task<User> UserDeleteAsync(Guid id, CancellationToken cancellationToken = default);

        #endregion
    }
}

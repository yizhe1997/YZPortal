using Application.Interfaces.Indexes;
using Application.Models;
using Application.Models.Graph;
using Application.Requests.Graph.Groups;

namespace Application.Interfaces.Services
{
    public interface IGraphService
    {
        #region Users

        Task<SearchResult<UserModel>> UsersToSearchResultAsync(ISearchParams request, System.Linq.Expressions.Expression<Func<UserModel, bool>>? searchPredicate = null, CancellationToken cancellationToken = default);

        Task<List<UserModel>> UsersGetAsync(string[]? select = null, string[]? orderBy = null, int pageSize = 10, int pageNumber = 1, CancellationToken cancellationToken = default);

        Task<Result<UserModel>> UserGetAsync(string? userSubId, CancellationToken cancellationToken = default);

        Task<Result> UserUpdateAsync<T>(string? userSubId, T body, CancellationToken cancellationToken = default);

        Task<Result> UserDeleteAsync(string? userSubId, CancellationToken cancellationToken = default);

        Task<List<GroupModel>> UserGroupsGetAsync(string? userSubId, string[]? select = null, string[]? orderBy = null, int pageSize = 10, int pageNumber = 1, CancellationToken cancellationToken = default);

        Task<SearchResult<GroupModel>> UserGroupsToSearchResultAsync(string? userSubId, ISearchParams request, System.Linq.Expressions.Expression<Func<GroupModel, bool>>? searchPredicate = null, CancellationToken cancellationToken = default);
        
        // TODO: make this better
        Task<string[]> UserGroupDisplayNamesGetAsync(string? userSubId, string[]? select = null, string[]? orderBy = null, int pageSize = 10, int pageNumber = 1, CancellationToken cancellationToken = default);
        
        #endregion

        #region Groups

        Task<List<GroupModel>> GroupsGetAsync(string[]? select = null, string[]? orderBy = null, int pageSize = 10, int pageNumber = 1, CancellationToken cancellationToken = default);

        Task<SearchResult<GroupModel>> GroupsToSearchResultAsync(ISearchParams request, System.Linq.Expressions.Expression<Func<GroupModel, bool>>? searchPredicate = null, CancellationToken cancellationToken = default);

        Task<List<UserModel>> GroupUsersGetAsync(string? groupId, string[]? select = null, string[]? orderBy = null, int pageSize = 10, int pageNumber = 1, CancellationToken cancellationToken = default);

        Task<SearchResult<UserModel>> GroupUsersToSearchResultAsync(string? groupId, ISearchParams request, System.Linq.Expressions.Expression<Func<UserModel, bool>>? searchPredicate = null, CancellationToken cancellationToken = default);

        // Ref https://learn.microsoft.com/en-us/graph/api/group-post-members?view=graph-rest-1.0&tabs=csharp
        Task<Result> GroupAddUsersAsync(AddUsersToGroupCommand request, CancellationToken cancellationToken = default);

        Task<Result> GroupRemoveUserAsync(RemoveUserFromGroupCommand request, CancellationToken cancellationToken = default);

        #endregion
    }
}

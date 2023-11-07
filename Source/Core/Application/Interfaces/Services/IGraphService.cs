using Application.Interfaces.Indexes;
using Application.Models;
using Application.Models.Graph;
using Application.Requests.Graph.Groups;

namespace Application.Interfaces.Services
{
    public interface IGraphService
    {
        #region Users

        Task<SearchResult<UserModel>> UsersToSearchResultAsync(ISearchParams request, System.Linq.Expressions.Expression<Func<UserModel, bool>>? searchPredicate = null, CancellationToken cancellationToken = new CancellationToken());

        Task<List<UserModel>> UsersGetAsync(string[]? select = null, string[]? orderBy = null, int pageSize = 10, int pageNumber = 1, CancellationToken cancellationToken = new CancellationToken());

        Task<Result<UserModel>> UserGetAsync(string? userSubId, CancellationToken cancellationToken = new CancellationToken());

        Task<Result> UserUpdateAsync<T>(string? userSubId, T body, CancellationToken cancellationToken = new CancellationToken());

        Task<Result> UserDeleteAsync(string? userSubId, CancellationToken cancellationToken = new CancellationToken());

        Task<List<GroupModel>> UserGroupsGetAsync(string? userSubId, string[]? select = null, string[]? orderBy = null, int pageSize = 10, int pageNumber = 1, CancellationToken cancellationToken = new CancellationToken());

        Task<SearchResult<GroupModel>> UserGroupsToSearchResultAsync(string? userSubId, ISearchParams request, System.Linq.Expressions.Expression<Func<GroupModel, bool>>? searchPredicate = null, CancellationToken cancellationToken = new CancellationToken());
        
        // TODO: make this better
        Task<string[]> UserGroupDisplayNamesGetAsync(string? userSubId, string[]? select = null, string[]? orderBy = null, int pageSize = 10, int pageNumber = 1, CancellationToken cancellationToken = new CancellationToken());
        
        #endregion

        #region Groups

        Task<List<GroupModel>> GroupsGetAsync(string[]? select = null, string[]? orderBy = null, int pageSize = 10, int pageNumber = 1, CancellationToken cancellationToken = new CancellationToken());

        Task<SearchResult<GroupModel>> GroupsToSearchResultAsync(ISearchParams request, System.Linq.Expressions.Expression<Func<GroupModel, bool>>? searchPredicate = null, CancellationToken cancellationToken = new CancellationToken());

        Task<List<UserModel>> GroupUsersGetAsync(string? groupId, string[]? select = null, string[]? orderBy = null, int pageSize = 10, int pageNumber = 1, CancellationToken cancellationToken = new CancellationToken());

        Task<SearchResult<UserModel>> GroupUsersToSearchResultAsync(string? groupId, ISearchParams request, System.Linq.Expressions.Expression<Func<UserModel, bool>>? searchPredicate = null, CancellationToken cancellationToken = new CancellationToken());

        // Ref https://learn.microsoft.com/en-us/graph/api/group-post-members?view=graph-rest-1.0&tabs=csharp
        Task<Result> GroupAddUsersAsync(AddUsersToGroupCommand request, CancellationToken cancellationToken = new CancellationToken());

        Task<Result> GroupRemoveUserAsync(RemoveUserFromGroupCommand request, CancellationToken cancellationToken = new CancellationToken());

        #endregion
    }
}

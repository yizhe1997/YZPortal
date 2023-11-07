using Application.Interfaces.Indexes;
using Application.Interfaces.Services;
using Application.Models;
using Application.Models.Graph;
using Application.Requests.Graph.Groups;
using AutoMapper;
using Azure.Identity;
using Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Kiota.Abstractions;

namespace Infrastructure.Services
{
    public class GraphService : IGraphService
    {
        // Variables
        private readonly AzureAdB2CManagementConfig _azureAdB2CManagementConfig;
        private readonly GraphConfig _graphConfig;
        private readonly IMapper _mapper;

        // Constructor
        public GraphService(IOptions<AzureAdB2CManagementConfig> azureAdB2CManagementConfig, IOptions<GraphConfig> graphConfig, IMapper mapper)
        {
            _azureAdB2CManagementConfig = azureAdB2CManagementConfig.Value;
            _graphConfig = graphConfig.Value;
            _mapper = mapper;
        }

        #region Users

        public async Task<SearchResult<UserModel>> UsersToSearchResultAsync(ISearchParams request, System.Linq.Expressions.Expression<Func<UserModel, bool>>? searchPredicate = null, CancellationToken cancellationToken = new CancellationToken())
        {
            var users = await UsersGetAsync(request.Select, request.OrderBy, request.PageSize, request.PageNumber, cancellationToken);

            var result = SearchResult<UserModel>.Success<UserModel>(request, users, _mapper, searchPredicate);

            return result;
        }

        public async Task<List<UserModel>> UsersGetAsync(string[]? select = null, string[]? orderBy = null, int pageSize = 10, int pageNumber = 1, CancellationToken cancellationToken = new CancellationToken())
        {
            var graphClient = GetGraphClient();

            // Construct the original and subsequent OdataNextLink URLs which contains all the query parameters present in the original request
            var usersResponse = await graphClient.Users.GetAsync(requestConfiguration =>
            {
                //// doesnt work with b2c tenant... this makes me want to resort to sync 
                //// but i think i should just remove the totalitem count for now, at least we still have total page?
                //// https://learn.microsoft.com/en-us/graph/aad-advanced-queries?tabs=csharp
                //// https://stackoverflow.com/questions/74763398/use-filter-query-parameter-on-groups-id-members-with-graph-api-for-azure-b2c
                //requestConfiguration.Headers.Add("ConsistencyLevel", "eventual");
                //requestConfiguration.QueryParameters.Filter = "startswith(displayName%2C+'J')"; // this is probably case specific?
                //requestConfiguration.QueryParameters.Search = searchString;
                //requestConfiguration.QueryParameters.Count = true;
                requestConfiguration.QueryParameters.Select = select ?? Array.Empty<string>();
                requestConfiguration.QueryParameters.Top = pageSize;
                requestConfiguration.QueryParameters.Orderby = orderBy ?? Array.Empty<string>();

            },
            cancellationToken);

            //// Get item counts. The @odata.count property will be present only in the first page of the paged data. 
            //// Cant get this to work atm because of limitations
            //var totalItems = usersResponse?.OdataCount;
            //var totalPages = usersResponse?.OdataCount / pageSize;

            // Get items for a particular page
            usersResponse = (UserCollectionResponse?)await GetItemsFromPageAsync(graphClient, usersResponse, pageNumber, cancellationToken);

            var result = usersResponse?.Value != null ? _mapper.Map<List<UserModel>>(usersResponse.Value) : new List<UserModel>();

            return result;
        }

        public async Task<Result<UserModel>> UserGetAsync(string? userSubId, CancellationToken cancellationToken = new CancellationToken())
        {
            var graphClient = GetGraphClient();

            var user = await graphClient.Users[userSubId].GetAsync(requestConfiguration => requestConfiguration.Headers.Add("ConsistencyLevel", "eventual"), cancellationToken);

            var result = user != null ? _mapper.Map<UserModel>(user) : new UserModel();

            return await Result<UserModel>.SuccessAsync(result);
        }

        public async Task<Result> UserUpdateAsync<T>(string? userSubId, T body, CancellationToken cancellationToken = new CancellationToken())
        {
            var graphClient = GetGraphClient();

            // Map fields to existing user and save
            var requestBody = _mapper.Map(body, new User());

            await graphClient.Users[userSubId].PatchAsync(requestBody, cancellationToken: cancellationToken);

            return await Result.SuccessAsync();
        }

        public async Task<Result> UserDeleteAsync(string? userSubId, CancellationToken cancellationToken = new CancellationToken())
        {
            var graphClient = GetGraphClient();

            await graphClient.Users[userSubId].DeleteAsync(requestConfiguration => requestConfiguration.Headers.Add("ConsistencyLevel", "eventual"), cancellationToken);

            return await Result.SuccessAsync();
        }

        public async Task<List<GroupModel>> UserGroupsGetAsync(string? userSubId, string[]? select = null, string[]? orderBy = null, int pageSize = 10, int pageNumber = 1, CancellationToken cancellationToken = new CancellationToken())
        {
            var graphClient = GetGraphClient();

            // Construct the original and subsequent OdataNextLink URLs which contains all the query parameters present in the original request
            var userGroupsResponse = await graphClient.Users[userSubId].MemberOf.GraphGroup.GetAsync(requestConfiguration =>
            {
                requestConfiguration.QueryParameters.Select = select ?? Array.Empty<string>();
                requestConfiguration.QueryParameters.Top = pageSize;
                requestConfiguration.QueryParameters.Orderby = orderBy ?? Array.Empty<string>();
            },
            cancellationToken);

            // Get items for a particular page
            userGroupsResponse = (GroupCollectionResponse?)await GetItemsFromPageAsync(graphClient, userGroupsResponse, pageNumber, cancellationToken);

            var result = userGroupsResponse?.Value != null ? _mapper.Map<List<GroupModel>>(userGroupsResponse.Value) : new List<GroupModel>();

            return result;
        }

        public async Task<SearchResult<GroupModel>> UserGroupsToSearchResultAsync(string? userSubId, ISearchParams request, System.Linq.Expressions.Expression<Func<GroupModel, bool>>? searchPredicate = null, CancellationToken cancellationToken = new CancellationToken())
        {
            var groups = await UserGroupsGetAsync(userSubId, request.Select, request.OrderBy, request.PageSize, request.PageNumber, cancellationToken);

            var result = SearchResult<GroupModel>.Success<GroupModel>(request, groups, _mapper, searchPredicate);

            return result;
        }

        public async Task<string[]> UserGroupDisplayNamesGetAsync(string? userSubId, string[]? select = null, string[]? orderBy = null, int pageSize = 10, int pageNumber = 1, CancellationToken cancellationToken = new CancellationToken()) =>
            (await UserGroupsGetAsync(userSubId, new string[] { nameof(Group.DisplayName) }, orderBy, pageSize, pageNumber, cancellationToken)).Select(x => x.DisplayName ?? string.Empty).ToArray() ?? Array.Empty<string>();

        #endregion

        #region Groups

        public async Task<List<GroupModel>> GroupsGetAsync(string[]? select = null, string[]? orderBy = null, int pageSize = 10, int pageNumber = 1, CancellationToken cancellationToken = new CancellationToken())
        {
            var graphClient = GetGraphClient();

            // Construct the original and subsequent OdataNextLink URLs which contains all the query parameters present in the original request
            var groupsResponse = await graphClient.Groups.GetAsync(requestConfiguration =>
            {
                requestConfiguration.QueryParameters.Select = select ?? Array.Empty<string>();
                requestConfiguration.QueryParameters.Top = pageSize;
                requestConfiguration.QueryParameters.Orderby = orderBy ?? Array.Empty<string>();
            },
            cancellationToken);

            // Get items for a particular page
            groupsResponse = (GroupCollectionResponse?)await GetItemsFromPageAsync(graphClient, groupsResponse, pageNumber, cancellationToken);

            var result = groupsResponse?.Value != null ? _mapper.Map<List<GroupModel>>(groupsResponse.Value) : new List<GroupModel>();

            return result;
        }

        public async Task<SearchResult<GroupModel>> GroupsToSearchResultAsync(ISearchParams request, System.Linq.Expressions.Expression<Func<GroupModel, bool>>? searchPredicate = null, CancellationToken cancellationToken = new CancellationToken())
        {
            var groups = await GroupsGetAsync(request.Select, request.OrderBy, request.PageSize, request.PageNumber, cancellationToken);

            var result = SearchResult<GroupModel>.Success<GroupModel>(request, groups, _mapper, searchPredicate);

            return result;
        }

        public async Task<List<UserModel>> GroupUsersGetAsync(string? groupId, string[]? select = null, string[]? orderBy = null, int pageSize = 10, int pageNumber = 1, CancellationToken cancellationToken = new CancellationToken())
        {
            var graphClient = GetGraphClient();

            // Construct the original and subsequent OdataNextLink URLs which contains all the query parameters present in the original request
            var usersInGroup = await graphClient.Groups[groupId].Members.GraphUser.GetAsync(requestConfiguration =>
            {
                requestConfiguration.QueryParameters.Select = select ?? Array.Empty<string>();
                requestConfiguration.QueryParameters.Top = pageSize;
                requestConfiguration.QueryParameters.Orderby = orderBy ?? Array.Empty<string>();
            },
            cancellationToken);

            // Get items for a particular page
            usersInGroup = (UserCollectionResponse?)await GetItemsFromPageAsync(graphClient, usersInGroup, pageNumber, cancellationToken);

            var result = usersInGroup?.Value != null ? _mapper.Map<List<UserModel>>(usersInGroup.Value) : new List<UserModel>();

            return result;
        }

        public async Task<SearchResult<UserModel>> GroupUsersToSearchResultAsync(string? groupId, ISearchParams request, System.Linq.Expressions.Expression<Func<UserModel, bool>>? searchPredicate = null, CancellationToken cancellationToken = new CancellationToken())
        {
            var users = await GroupUsersGetAsync(groupId, request.Select, request.OrderBy, request.PageSize, request.PageNumber, cancellationToken);
            
            var result = SearchResult<UserModel>.Success<UserModel>(request, users, _mapper, searchPredicate);

            return result;
        }

        // Ref https://learn.microsoft.com/en-us/graph/api/group-post-members?view=graph-rest-1.0&tabs=csharp
        public async Task<Result> GroupAddUsersAsync(AddUsersToGroupCommand request, CancellationToken cancellationToken = new CancellationToken())
        {
            var graphClient = GetGraphClient();

            // Note that up to 20 members can be added in a single request.
            var formattedUserIds = new List<string>();
            foreach (var userId in request.UserSubjectIds)
            {
                formattedUserIds.Add($"{_graphConfig.BaseUrl}/directoryObjects/{userId}");
            }
            var requestBody = new Group
            {
                AdditionalData = new Dictionary<string, object>
                {
                    {
                        "members@odata.bind" , formattedUserIds
                    },
                },
            };

            await graphClient.Groups[request.GroupId].PatchAsync(requestBody, cancellationToken: cancellationToken);

            return await Result.SuccessAsync();
        }

        public async Task<Result> GroupRemoveUserAsync(RemoveUserFromGroupCommand request, CancellationToken cancellationToken = new CancellationToken())
        {
            var graphClient = GetGraphClient();

            await graphClient.Groups[request.GroupId].Members[request.UserSubjectId].Ref.DeleteAsync(cancellationToken: cancellationToken);

            return await Result.SuccessAsync();
        }

        #endregion

        #region Helpers

        private GraphServiceClient GetGraphClient()
        {
            // using Azure.Identity;
            var options = new TokenCredentialOptions
            {
                AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
            };

            // https://learn.microsoft.com/dotnet/api/azure.identity.clientsecretcredential
            var clientSecretCredential = new ClientSecretCredential(_azureAdB2CManagementConfig.TenantId, _azureAdB2CManagementConfig.ClientId, _azureAdB2CManagementConfig.ClientSecret, options);

            var graphClient = new GraphServiceClient(clientSecretCredential, _graphConfig.Scopes);

            return graphClient;
        }

        // REF: https://stackoverflow.com/questions/75690753/pagination-in-ms-graph-groups-in-sdk-v5
        private static async Task<BaseCollectionPaginationCountResponse?> GetItemsFromPageAsync(GraphServiceClient graphClient, BaseCollectionPaginationCountResponse? collectionPage, int requestedPageNumber, CancellationToken cancellationToken = new CancellationToken())
        {
            var pageCount = 1;
            var nextPageLink = collectionPage?.OdataNextLink;

            // Iterate over the pages only if 
            while (nextPageLink != null && pageCount != requestedPageNumber)
            {
                var nextPageRequestInformation = new RequestInformation
                {
                    HttpMethod = Method.GET,
                    UrlTemplate = nextPageLink
                };

                collectionPage = await graphClient.RequestAdapter.SendAsync(nextPageRequestInformation, (parseNode) => new UserCollectionResponse(), cancellationToken: cancellationToken);
                nextPageLink = collectionPage?.OdataNextLink;
                pageCount++;
            }

            return collectionPage;
        }

        #endregion
    }
}

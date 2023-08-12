using AutoMapper;
using Azure.Identity;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Kiota.Abstractions;
using YZPortal.Core.Indexes;
using YZPortal.FullStackCore.Requests.Indexes;

namespace YZPortal.Core.Graph
{
    // TODO: do ODataError in error pipeline instead....
    public class GraphClientProvider
    {
        // Variables
        private readonly AzureAdB2CManagementOptions _azureAdB2CManagementOptions;
        private readonly GraphOptions _graphOptions;
        private readonly IMapper _mapper;

        // Constructor
        public GraphClientProvider(IOptions<AzureAdB2CManagementOptions> azureAdB2CManagementOptions, IOptions<GraphOptions> graphOptions, IMapper mapper)
        {
            _azureAdB2CManagementOptions = azureAdB2CManagementOptions.Value;
            _graphOptions = graphOptions.Value;
            _mapper = mapper;
        }

        #region Users

        public async Task<SearchList<User>> UsersToSearchListAsync(ISearchParams request, System.Linq.Expressions.Expression<Func<User, bool>>? searchPredicate = null, CancellationToken cancellationToken = new CancellationToken())
        {
            var users = await UsersGetAsync(request.Select, request.OrderBy, request.PageSize, request.PageNumber, cancellationToken);

            // AsQueryable allows dynamically build and refine query by adding additional LINQ operators
            return users.CreateSearchList(request, searchPredicate);
        }

        public async Task<List<User>> UsersGetAsync(string[]? select = null, string[]? orderBy = null, int pageSize = 10, int pageNumber = 1, CancellationToken cancellationToken = new CancellationToken())
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

            return usersResponse?.Value ?? new List<User>();
        }

        public async Task<User> UserGetAsync(string? userSubId, CancellationToken cancellationToken = new CancellationToken())
        {
            var graphClient = GetGraphClient();

            var user = await graphClient.Users[userSubId].GetAsync(requestConfiguration => requestConfiguration.Headers.Add("ConsistencyLevel", "eventual"), cancellationToken);

            return user ?? new User();
        }

        public async Task<User> UserUpdateAsync<T>(string? userSubId, T body, CancellationToken cancellationToken = new CancellationToken())
        {
            var graphClient = GetGraphClient();

            // Map fields to existing user and save
            var requestBody = _mapper.Map(body, new User());

            var user = await graphClient.Users[userSubId].PatchAsync(requestBody, cancellationToken : cancellationToken);

            return user ?? new User();
        }


        public async Task<User> UserDeleteAsync(string? userSubId, CancellationToken cancellationToken = new CancellationToken())
        {
            var graphClient = GetGraphClient();

            var user = await graphClient.Users[userSubId].GetAsync(requestConfiguration => requestConfiguration.Headers.Add("ConsistencyLevel", "eventual"), cancellationToken);

            if (user != null)
                await graphClient.Users[userSubId].DeleteAsync(requestConfiguration => requestConfiguration.Headers.Add("ConsistencyLevel", "eventual"), cancellationToken);

            return user ?? new User();
        }

        public async Task<List<Group>> UserGroupsGetAsync(string? userSubId, string[]? select = null, string[]? orderBy = null, int pageSize = 10, int pageNumber = 1, CancellationToken cancellationToken = new CancellationToken())
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

            return userGroupsResponse?.Value ?? new List<Group>();
        }

        public async Task<SearchList<Group>> UserGroupsToSearchListAsync(string? userSubId, ISearchParams request, System.Linq.Expressions.Expression<Func<Group, bool>>? searchPredicate = null, CancellationToken cancellationToken = new CancellationToken())
        {
            var groups = await UserGroupsGetAsync(userSubId, request.Select, request.OrderBy, request.PageSize, request.PageNumber, cancellationToken);

            return groups.CreateSearchList(request, searchPredicate);
        }

        #endregion

        #region Groups

        public async Task<List<Group>> GroupsGetAsync(string[]? select = null, string[]? orderBy = null, int pageSize = 10, int pageNumber = 1, CancellationToken cancellationToken = new CancellationToken())
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

            return groupsResponse?.Value ?? new List<Group>();
        }

        public async Task<SearchList<Group>> GroupsToSearchListAsync(ISearchParams request, System.Linq.Expressions.Expression<Func<Group, bool>>? searchPredicate = null, CancellationToken cancellationToken = new CancellationToken())
        {
            var groups = await GroupsGetAsync(request.Select, request.OrderBy, request.PageSize, request.PageNumber, cancellationToken);

            // TODO: use the correct extension
            return groups.CreateSearchList(request, searchPredicate);
        }

        public async Task<List<User>> GroupUsersGetAsync(string? groupId, string[]? select = null, string[]? orderBy = null, int pageSize = 10, int pageNumber = 1, CancellationToken cancellationToken = new CancellationToken())
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

            return usersInGroup?.Value ?? new List<User>();
        }

        // Ref https://learn.microsoft.com/en-us/graph/api/group-post-members?view=graph-rest-1.0&tabs=csharp
        public async Task GroupAddUsersAsync(string? groupId, string[] userSubIds, CancellationToken cancellationToken = new CancellationToken())
        {
            var graphClient = GetGraphClient();

            // Note that up to 20 members can be added in a single request.
            var formattedUserIds = new List<string>();
            foreach (var userId in userSubIds)
            {
                formattedUserIds.Add($"{_graphOptions.BaseUrl}/directoryObjects/{userId}");
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

            await graphClient.Groups[groupId].PatchAsync(requestBody, cancellationToken: cancellationToken);

            return;
        }

        public async Task GroupRemoveUserAsync(string? groupId, string? userSubId, CancellationToken cancellationToken = new CancellationToken())
        {
            var graphClient = GetGraphClient();

            await graphClient.Groups[groupId].Members[userSubId].Ref.DeleteAsync(cancellationToken: cancellationToken);

            return;
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
            var clientSecretCredential = new ClientSecretCredential(_azureAdB2CManagementOptions.TenantId, _azureAdB2CManagementOptions.ClientId, _azureAdB2CManagementOptions.ClientSecret, options);

            var graphClient = new GraphServiceClient(clientSecretCredential, _graphOptions.Scopes);

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

using Azure.Identity;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Graph.Models.ODataErrors;
using Microsoft.Kiota.Abstractions;
using YZPortal.Core.Error;
using YZPortal.Core.Indexes;

namespace YZPortal.Core.Graph
{
    public class GraphClientProvider
    {
        // Variables
        private readonly AzureAdB2CManagementOptions _azureAdB2CManagementOptions;
        private readonly GraphOptions _graphOptions;

        // Constructor
        public GraphClientProvider(IOptions<AzureAdB2CManagementOptions> azureAdB2CManagementOptions, IOptions<GraphOptions> graphOptions)
        {
            _azureAdB2CManagementOptions = azureAdB2CManagementOptions.Value;
            _graphOptions = graphOptions.Value;
        }

        #region Users

        public async Task<SearchList<User>> UsersToSearchListAsync(ISearchParams request, System.Linq.Expressions.Expression<Func<User, bool>>? searchPredicate = null, CancellationToken cancellationToken = new CancellationToken())
        {
            var users = await UsersGetAsync(request.Select, request.OrderBy, request.PageSize, request.PageNumber);

            // AsQueryable allows dynamically build and refine query by adding additional LINQ operators
            return await users.AsQueryable().CreateSearchListAsync(request, searchPredicate, cancellationToken);
        }

        public async Task<List<User>> UsersGetAsync(string[]? select = null, string[]? orderBy = null, int pageSize = 10, int pageNumber = 1)
        {
            var graphClient = GetGraphClient();

            try
            {
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
                    
                });

                //// Get item counts. The @odata.count property will be present only in the first page of the paged data. 
                //// Cant get this to work atm because of limitations
                //var totalItems = usersResponse?.OdataCount;
                //var totalPages = usersResponse?.OdataCount / pageSize;

                // Get items for a particular page
                usersResponse = (UserCollectionResponse?)await GetItemsFromPageAsync(graphClient, usersResponse, pageNumber);

                return usersResponse?.Value ?? new List<User>();
            }
            catch (ODataError odataError)
            {
                throw new RestException(odataError.Error?.Code, odataError.Error?.Message);
            }
        }

        public async Task<User> UserGetAsync(string? userId)
        {
            var graphClient = GetGraphClient();

            try
            {
                var user = await graphClient
                    .Users[userId]
                    .GetAsync(requestConfiguration => requestConfiguration.Headers.Add("ConsistencyLevel", "eventual"));

                return user ?? new User();
            }
            catch (ODataError odataError)
            {
                throw new RestException(odataError.Error?.Code, odataError.Error?.Message);
            }
        }

        public async Task<User> UserDeleteAsync(string? userId)
        {
            var graphClient = GetGraphClient();

            try
            {
                var user = await graphClient
                    .Users[userId]
                    .GetAsync(requestConfiguration => requestConfiguration.Headers.Add("ConsistencyLevel", "eventual"));

                return user ?? new User();
            }
            catch (ODataError odataError)
            {
                throw new RestException(odataError.Error?.Code, odataError.Error?.Message);
            }
        }

        public async Task<List<Group>> UserGroupsGetAsync(string? userId, string[]? select = null, string[]? orderBy = null, int pageSize = 10, int pageNumber = 1)
        {
            var graphClient = GetGraphClient();

            try
            {
                // Construct the original and subsequent OdataNextLink URLs which contains all the query parameters present in the original request
                var userGroupsResponse = await graphClient.Users[userId].MemberOf.GraphGroup.GetAsync(requestConfiguration =>
                {
                    requestConfiguration.QueryParameters.Select = select ?? Array.Empty<string>();
                    requestConfiguration.QueryParameters.Top = pageSize;
                    requestConfiguration.QueryParameters.Orderby = orderBy ?? Array.Empty<string>();
                });

                // Get item counts. The @odata.count property will be present only in the first page of the paged data.
                var totalItems = userGroupsResponse?.OdataCount;
                var totalPages = userGroupsResponse?.OdataCount / pageSize;

                // Get items for a particular page
                userGroupsResponse = (GroupCollectionResponse?)await GetItemsFromPageAsync(graphClient, userGroupsResponse, pageNumber);

                return userGroupsResponse?.Value ?? new List<Group>();
            }
            catch (ODataError odataError)
            {
                throw new RestException(odataError.Error?.Code, odataError.Error?.Message);
            }
        }

        public async Task<SearchList<Group>> UserGroupsToSearchListAsync(string? userId, ISearchParams request, System.Linq.Expressions.Expression<Func<Group, bool>>? searchPredicate = null, CancellationToken cancellationToken = new CancellationToken())
        {
            var groups = await UserGroupsGetAsync(userId, request.Select, request.OrderBy, request.PageSize, request.PageNumber);

            return await groups.AsQueryable().CreateSearchListAsync(request, searchPredicate, cancellationToken);
        }

        #endregion

        #region Groups

        public async Task<List<Group>> GroupsGetAsync(string[]? select = null, string[]? orderBy = null, int pageSize = 10, int pageNumber = 1)
        {
            var graphClient = GetGraphClient();

            try
            {
                // Construct the original and subsequent OdataNextLink URLs which contains all the query parameters present in the original request
                var groupsResponse = await graphClient.Groups.GetAsync(requestConfiguration =>
                {
                    requestConfiguration.QueryParameters.Select = select ?? Array.Empty<string>();
                    requestConfiguration.QueryParameters.Top = pageSize;
                    requestConfiguration.QueryParameters.Orderby = orderBy ?? Array.Empty<string>();
                });

                // Get item counts. The @odata.count property will be present only in the first page of the paged data.
                var totalItems = groupsResponse?.OdataCount;
                var totalPages = groupsResponse?.OdataCount / pageSize;

                // Get items for a particular page
                groupsResponse = (GroupCollectionResponse?)await GetItemsFromPageAsync(graphClient, groupsResponse, pageNumber);

                return groupsResponse?.Value ?? new List<Group>();
            }
            catch (ODataError odataError)
            {
                throw new RestException(odataError.Error?.Code, odataError.Error?.Message);
            }
        }

        public async Task<SearchList<Group>> GroupsToSearchListAsync(ISearchParams request, System.Linq.Expressions.Expression<Func<Group, bool>>? searchPredicate = null, CancellationToken cancellationToken = new CancellationToken())
        {
            var groups = await GroupsGetAsync(request.Select, request.OrderBy, request.PageSize, request.PageNumber);

            // TODO: use the correct extension
            return await groups.AsQueryable().CreateSearchListAsync(request, searchPredicate, cancellationToken);
        }

        public async Task<List<User>> GroupUsersGetAsync(string groupId, string[]? select = null, string[]? orderBy = null, int pageSize = 10, int pageNumber = 1)
        {
            var graphClient = GetGraphClient();

            try
            {
                // Construct the original and subsequent OdataNextLink URLs which contains all the query parameters present in the original request
                var usersInGroup = await graphClient.Groups[groupId].Members.GraphUser.GetAsync(requestConfiguration =>
                {
                    requestConfiguration.QueryParameters.Select = select ?? Array.Empty<string>();
                    requestConfiguration.QueryParameters.Top = pageSize;
                    requestConfiguration.QueryParameters.Orderby = orderBy ?? Array.Empty<string>();
                });

                // Get item counts. The @odata.count property will be present only in the first page of the paged data.
                var totalItems = usersInGroup?.OdataCount;
                var totalPages = usersInGroup?.OdataCount / pageSize;

                // Get items for a particular page
                usersInGroup = (UserCollectionResponse?)await GetItemsFromPageAsync(graphClient, usersInGroup, pageNumber);

                return usersInGroup?.Value ?? new List<User>();
            }
            catch (ODataError odataError)
            {
                throw new RestException(odataError.Error?.Code, odataError.Error?.Message);
            }
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
        private static async Task<BaseCollectionPaginationCountResponse?> GetItemsFromPageAsync(GraphServiceClient graphClient, BaseCollectionPaginationCountResponse? collectionPage, int requestedPageNumber)
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

                collectionPage = await graphClient.RequestAdapter.SendAsync(nextPageRequestInformation, (parseNode) => new UserCollectionResponse());
                nextPageLink = collectionPage?.OdataNextLink;
                pageCount++;
            }

            return collectionPage;
        }

        #endregion
    }
}

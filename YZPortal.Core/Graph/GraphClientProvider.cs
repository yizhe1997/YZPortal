using Azure.Identity;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Graph.Models.ODataErrors;
using System;
using System.Net;
using YZPortal.Core.Error;

namespace YZPortal.Core.Graph
{
    public class GraphClientProvider
    {
        private readonly AzureAdB2CManagementOptions _azureAdB2CManagementOptions;
        private readonly GraphOptions _graphOptions;

        public GraphClientProvider(IOptions<AzureAdB2CManagementOptions> azureAdB2CManagementOptions, IOptions<GraphOptions> graphOptions)
        {
            _azureAdB2CManagementOptions = azureAdB2CManagementOptions.Value;
            _graphOptions = graphOptions.Value;
        }

        public GraphServiceClient GetGraphClient()
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

        #region Users

        public async Task<List<User>> GetUsers(string[] orderBy = null, int pageSize = 10, int pageNumber = 1, string? searchString = null)
        {
            var graphClient = GetGraphClient();
            var count = 0;
            var userList = new List<User>();

            try
            {
                var usersResponse = await graphClient.Users.GetAsync(requestConfiguration =>
                {
                    //requestConfiguration.QueryParameters.Select = new string[] { "id", "createdDateTime", "displayName" };
                    //requestConfiguration.QueryParameters.Expand = new string[] { "members" };
                    requestConfiguration.QueryParameters.Top = pageSize;
                    requestConfiguration.QueryParameters.Orderby = orderBy ?? new string[] { };
                    //requestConfiguration.QueryParameters.Filter = "startswith(displayName%2C+'J')"; // this is probably case specific?
                    requestConfiguration.QueryParameters.Search = searchString;
                    requestConfiguration.QueryParameters.Count = true;
                });

                var pageIterator = PageIterator<User, UserCollectionResponse>
                .CreatePageIterator(graphClient, usersResponse, (user) =>
                {
                    count++;
                    if ((count > pageSize * (pageNumber - 1)) || (count <= pageSize * pageNumber))
                    {
                        userList.Add(user);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                });

                await pageIterator.IterateAsync();

                return userList;
            }
            catch (ODataError odataError)
            {
                throw new RestException(odataError.Error.Code, odataError.Error.Message);
            }
        }

        public async Task<User> GetUser(string userId)
        {
            var graphClient = GetGraphClient();

            try
            {
                var user = await graphClient
                    .Users[userId]
                    .GetAsync(requestConfiguration => requestConfiguration.Headers.Add("ConsistencyLevel", "eventual"));

                return user;
            }
            catch (ODataError odataError)
            {
                throw new RestException(odataError.Error.Code, odataError.Error.Message);
            }
        }

        public async Task<List<Group>> GetUserGroups(string userId, string[] select = null, string[] orderBy = null, int pageSize = 10, int pageNumber = 1, string? searchString = null)
        {
            var graphClient = GetGraphClient();
            var count = 0;

            try
            {
                var usersResponse = await graphClient.Users[userId].MemberOf.GraphGroup.GetAsync(requestConfiguration =>
                {
                    requestConfiguration.QueryParameters.Select = select ?? Array.Empty<string>();
                    //requestConfiguration.QueryParameters.Expand = new string[] { "members" };
                    requestConfiguration.QueryParameters.Top = pageSize;
                    requestConfiguration.QueryParameters.Orderby = orderBy ?? Array.Empty<string>();
                    //requestConfiguration.QueryParameters.Filter = "startswith(displayName%2C+'J')"; // this is probably case specific?
                    requestConfiguration.QueryParameters.Search = searchString;
                    requestConfiguration.QueryParameters.Count = true;
                });

                return usersResponse?.Value ?? new List<Group>();
            }
            catch (ODataError odataError)
            {
                throw new RestException(odataError.Error.Code, odataError.Error.Message);
            }
        }

        #endregion

        #region

        public async Task<List<Group>> GetGroups(string[] orderBy = null, int pageSize = 10, int pageNumber = 1, string? searchString = null)
        {
            var graphClient = GetGraphClient();
            var count = 0;
            var groupList = new List<Group>();

            try
            {
                var groupsResponse = await graphClient.Groups.GetAsync(requestConfiguration =>
                {
                    //requestConfiguration.QueryParameters.Select = new string[] { "id", "createdDateTime", "displayName" };
                    requestConfiguration.QueryParameters.Expand = new string[] { "members" };
                    requestConfiguration.QueryParameters.Top = pageSize;
                    requestConfiguration.QueryParameters.Orderby = orderBy ?? new string[] { };
                    //requestConfiguration.QueryParameters.Filter = "startswith(displayName%2C+'J')"; // this is probably case specific?
                    requestConfiguration.QueryParameters.Search = searchString;
                    requestConfiguration.QueryParameters.Count = true;
                });

                var pageIterator = PageIterator<Group, GroupCollectionResponse>
                .CreatePageIterator(graphClient, groupsResponse, (group) =>
                {
                    count++;
                    if ((count > pageSize * (pageNumber - 1)) || (count <= pageSize * pageNumber))
                    {
                        groupList.Add(group);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                });

                await pageIterator.IterateAsync();

                return groupList;
            }
            catch (ODataError odataError)
            {
                throw new RestException(odataError.Error.Code, odataError.Error.Message);
            }
        }

        public async Task<List<User>> GetGroupUsers(string groupId, string[] orderBy = null, int pageSize = 10, int pageNumber = 1, string? searchString = null)
        {
            var graphClient = GetGraphClient();
            var count = 0;
            var userList = new List<User>();

            try
            {
                var usersInGroup = await graphClient.Groups[groupId].Members.GraphUser.GetAsync(requestConfiguration =>
                {
                    requestConfiguration.QueryParameters.Top = pageSize;
                    requestConfiguration.QueryParameters.Orderby = orderBy ?? new string[] { };
                    //requestConfiguration.QueryParameters.Filter = "startswith(displayName%2C+'J')"; // this is probably case specific?
                    requestConfiguration.QueryParameters.Search = searchString;
                    requestConfiguration.QueryParameters.Count = true;
                });

                var pageIterator = PageIterator<User, UserCollectionResponse>
                .CreatePageIterator(graphClient, usersInGroup, (user) =>
                {
                    count++;
                    if ((count > pageSize * (pageNumber - 1)) || (count <= pageSize * pageNumber))
                    {
                        userList.Add(user);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                });

                await pageIterator.IterateAsync();

                return userList;
            }
            catch (ODataError odataError)
            {
                throw new RestException(odataError.Error.Code, odataError.Error.Message);
            }
        }

        #endregion
    }
}

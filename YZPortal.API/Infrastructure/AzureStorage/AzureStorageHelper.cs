using System.Net;
using YZPortal.Core.Error;

namespace YZPortal.API.Infrastructure.AzureStorage
{
    public static class AzureStorageHelper
    {
        /// <summary>
        ///     Throws exception if container name option is null or empty else returns itself. Primarily for displaying a common error message
        /// </summary>
        public static string CheckIfContainerNameIsNullOrEmpty(this string containerValue, string containerName)
        {
            if (string.IsNullOrEmpty(containerValue))
                throw new RestException(HttpStatusCode.InternalServerError, $"Configuration for {containerName} is empty or null, please contact admin!");

            return containerValue;
        }
    }
}

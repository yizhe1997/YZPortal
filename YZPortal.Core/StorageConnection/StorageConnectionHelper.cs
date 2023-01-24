using System.Net;
using YZPortal.Core.Error;

namespace YZPortal.Core.StorageConnection
{
    public static class StorageConnectionHelper
    {
        /// <summary>
        ///     Returns the attribute value for a connection string. e.g. "AccountKey" of azure storage connection
        ///     string. Separates the connection string via delim and looks for the required attribute. Throws rest 
        ///     exception if the connection string is empty. 
        /// </summary>
        /// <param name="attribute">Attribute key tgter with the delim separating the value e.g. "=" or ":"</param>
        /// <param name="delimiter">Delim separating the attribute key value pair</param>
        /// <param name="connectionString">The connection string set in config</param>
        /// <returns></returns>
        public static string GetConnectionStringAttribute(string connectionString, string delimiter, string attribute)
        {
            if (!string.IsNullOrEmpty(connectionString))
            {
                string[] parts = connectionString.Split(delimiter);

                for (int i = 0; i < parts.Length; i++)
                {
                    string part = parts[i].Trim();
                    if (part.StartsWith(attribute))
                    {
                        return part.Replace(attribute, "");
                    }
                }
                throw new RestException(HttpStatusCode.InternalServerError, $"Unable to find {attribute.Remove(attribute.Length - 1)} attribute for storage connection string, please contact admin!");
            }
            else
                throw new RestException(HttpStatusCode.InternalServerError, $"Attribute value for {attribute.Remove(attribute.Length - 1)} storage is empty or null, please contact admin!");
        }
    }
}

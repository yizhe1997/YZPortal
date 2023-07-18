using System.Web;

namespace YZPortal.FullStackCore.Extensions
{
    public static class HttpRequestMessageExtensions
	{
		public static void AddQueryParam(this HttpRequestMessage request, string name, string? value)
		{
            var uriBuilder = new UriBuilder(request.RequestUri ?? new Uri(string.Empty));
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query[name] = value;
            uriBuilder.Query = query.ToString();
            request.RequestUri = uriBuilder.Uri;
        }
	}
}

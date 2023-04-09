using System.Net.Http.Headers;
using System.Web;

namespace YZPortal.FullStackCore.Extensions
{
	public static class HttpRequestMessageExtensions
	{
		public static void AddBearerToken(this HttpRequestMessage request, string token)
		{
			if (!string.IsNullOrEmpty(token))
			{
				request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
			}
		}

		public static void AddQueryParam(this HttpRequestMessage request, string name, string value)
		{
			if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
			{
				var uriBuilder = new UriBuilder(request.RequestUri);
				var query = HttpUtility.ParseQueryString(uriBuilder.Query);
				query[name] = value;
				uriBuilder.Query = query.ToString();
				request.RequestUri = uriBuilder.Uri;
			}
		}
	}
}

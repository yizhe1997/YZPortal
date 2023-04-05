using System.Net.Http.Headers;

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
	}
}

using Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Infrastructure.Extensions
{
    public static class IEndpointRouteBuilderExtensions
    {
        public static void MapNotifications(this IEndpointRouteBuilder builder)
        {
            builder.MapHub<ChatHubService>("/chat", options =>
            {
                options.CloseOnAuthenticationExpiration = true;
            });
        }
    }
}

using Infrastructure.Extensions;

namespace YZPortal.API.Extensions
{
    public static class IEndpointRouteBuilderExtensions
    {
        public static void MapEndpoints(this IEndpointRouteBuilder builder)
        {
            builder.MapControllers();

            builder.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            builder.MapRazorPages();
            builder.MapNotifications();
        }
    }
}

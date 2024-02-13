using Hangfire.Dashboard;

namespace Infrastructure.Services.BackgroundJob
{
    public class HangFireDashboardAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public HangFireDashboardAuthorizationFilter()
        {
        }

        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            // using auth policy for privilage 
            return httpContext.User.Identity?.IsAuthenticated ?? false;
        }
    }
}

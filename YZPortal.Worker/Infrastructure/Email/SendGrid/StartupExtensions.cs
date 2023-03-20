using SendGrid.Core;

namespace YZPortal.Worker.Infrastructure.Email.SendGrid
{
	public static class StartupExtensions
	{
		public static void AddSendGridNotifications(this IServiceCollection services, IConfiguration configuration)
		{
			services.Configure<SendGridOptions>(configuration.GetSection("SendGrid"));
			
			var options = configuration.GetSection("SendGrid").Get<SendGridOptions>();
			if (options != null) 
				SendGridServer.Authorize(options.ApiKey);
		}
	}
}

using Microsoft.Extensions.Options;
using YZPortal.Core.Domain.Contexts;
using YZPortal.Core.Domain.Database.EntityTypes.Auditable;
using YZPortal.Worker.Infrastructure.Email;
using YZPortal.Worker.Infrastructure.Email.OfficeSmtp;
using YZPortal.Worker.Infrastructure.Email.SendGrid;
using YZPortal.Worker.Infrastructure.ScheduledTasks;

namespace YZPortal.Worker.Tasks.Email.Users
{
    public class SendPasswordResetsTask : EmailTask
    {
        public SendPasswordResetsTask(IServiceScopeFactory serviceScopeFactory, IOptions<ScheduledTasksOptions> options, IOptions<SendGridOptions> sendGridOptions, IOptions<EmailOptions> emailOptions, IOptions<OfficeSmtpOptions> officeSmtpOptions) : base(serviceScopeFactory, options, sendGridOptions, emailOptions, officeSmtpOptions)
        {
        }

        public override async Task ProcessInScope(IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            var dbContext = serviceProvider.GetRequiredService<PortalContext>();
            var logger = serviceProvider.GetRequiredService<ILogger<SendPasswordResetsTask>>();

            await SendPasswordResetsAsync(dbContext, logger, cancellationToken);
        }

        public async Task SendPasswordResetsAsync(PortalContext dbContext, ILogger<SendPasswordResetsTask> logger, CancellationToken cancellationToken)
        {
            logger.LogInformation("Sending password resets ...");

            try
            {
                //// Parse and validate template
                //var template = ScribanHelper.ParseTemplate("./SendPasswordReset.html");
                //if (template == null)
                //    throw new Exception("Failed to parse template for " + nameof(SendPasswordResetsTask));

                //// Query
                //var lastAttempted = DateTime.UtcNow.Subtract(emailOptions.AttemptInterval);
                //var resets = await dbContext.UserPasswordResets
                //    .Include(x => x.User)
                //    .Where(x =>
                //        x.UserId != Guid.Empty && // make sure there is an associated user for the email AND
                //        (x.SentDateTime == null && (x.ValidUntilDateTime > DateTime.UtcNow || x.ValidUntilDateTime == null)) || // (has not been sent AND is still valid) OR
                //        (
                //            (x.FailedSentDateTime != null && x.SentDateTime == null) &&// (it has failed AND
                //            x.Attempts < emailOptions.SendAttempts && // has not been attempted too often AND                     
                //            x.LastAttemptedSentDateTime < lastAttempted // enough time has passed since last attempt)
                //        )
                //    ).ToListAsync();

                //// Prepare template input
                //var models = resets.Select(x => new EmailReset
                //{
                //    callbackurl = x.CallbackUrl,
                //    Notification = x
                //}).ToList();

                //// Send notifications and update emailable entities
                //var result = await SendNotificationsAsync(models.Select(x => new EmailMessage
                //{
                //    Content = template.Render(x),
                //    Subject = "Dealer Portal Password Reset",
                //    Notifications = new List<EmailableEntity>() { x.Notification }
                //}).ToList(), cancellationToken);

                //await dbContext.BulkUpdateAsync(resets);

                //logger.LogInformation($"Password resets sent: ({result})");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error while sending password resets: {ex.Message}");
            }
        }

        class EmailReset
        {
            public string? callbackurl { get; set; }
            public EmailableEntity? Notification { get; set; }
        }
    }
}

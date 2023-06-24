using Microsoft.Extensions.Options;
using YZPortal.Core.Domain.Contexts;
using YZPortal.Core.Domain.Database.EntityTypes.Auditable;
using YZPortal.Worker.Infrastructure.Email;
using YZPortal.Worker.Infrastructure.Email.OfficeSmtp;
using YZPortal.Worker.Infrastructure.Email.SendGrid;
using YZPortal.Worker.Infrastructure.ScheduledTasks;

namespace YZPortal.Worker.Tasks.Email.Memberships
{
    public class SendMembershipNotificationsTask : EmailTask
    {
        public SendMembershipNotificationsTask(IServiceScopeFactory serviceScopeFactory, IOptions<ScheduledTasksOptions> options, IOptions<SendGridOptions> sendGridOptions, IOptions<EmailOptions> emailOptions, IOptions<OfficeSmtpOptions> officeSmtpOptions) : base(serviceScopeFactory, options, sendGridOptions, emailOptions, officeSmtpOptions)
        {
        }

        public override async Task ProcessInScope(IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            var dbContext = serviceProvider.GetRequiredService<PortalContext>();
            var logger = serviceProvider.GetRequiredService<ILogger<SendMembershipNotificationsTask>>();

            await SendMembershipNotificationsAsync(dbContext, logger, cancellationToken);
        }

        public async Task SendMembershipNotificationsAsync(PortalContext dbContext, ILogger<SendMembershipNotificationsTask> logger, CancellationToken cancellationToken)
        {
            logger.LogInformation("Sending membership notifications ...");

            try
            {
                //// Parse and validate template
                //var template = ScribanHelper.ParseTemplate("./SendMembershipNotification.html");
                //if (template == null)
                //    throw new Exception("Failed to parse template for " + nameof(SendMembershipNotificationsTask));

                //// Query
                //var lastAttempted = DateTime.UtcNow.Subtract(emailOptions.AttemptInterval);
                //var notifications = await dbContext.MembershipNotifications
                //.Include(x => x.Membership)
                //.ThenInclude(x => x.Dealer)
                //.Where(x =>
                //    x.SentDateTime == null || // has not been sent OR 
                //    (
                //        x.FailedSentDateTime != null && x.SentDateTime == null && // (it has failed AND
                //        x.Attempts < emailOptions.SendAttempts && // has not been attempted too often AND                      
                //        x.LastAttemptedSentDateTime < lastAttempted // enough time has passed since last attempt)
                //    )
                //).ToListAsync();

                //// Prepare template input
                //var models = notifications
                //    .GroupBy(x => x.Email)
                //    .Select(x => new EmailMembershipNotification
                //    {
                //        dealername = string.Join(", ", x.ToList().Select(x => x.Membership.Dealer.Name).ToList()),
                //        Notifications = x.ToList()
                //    }).ToList();

                //// Send notifications and update emailable entities
                //var result = await SendNotificationsAsync(models.Select(x => new EmailMessage
                //{
                //    Content = template.Render(x),
                //    Subject = "YZ Portal Membership Notification",
                //    Notifications = x.Notifications
                //}).ToList(), cancellationToken);

                //await dbContext.BulkUpdateAsync(notifications);

                //logger.LogInformation($"Membership notifications sent: ({result})");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error while sending membership notifications: {ex.Message}");
            }
        }

        class EmailMembershipNotification
        {
            public string? dealername { get; set; }
            public IEnumerable<EmailableEntity> Notifications { get; set; } = new List<EmailableEntity>();
        }
    }
}

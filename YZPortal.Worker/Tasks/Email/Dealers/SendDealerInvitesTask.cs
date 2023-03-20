using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using YZPortal.Core.Domain.Contexts;
using YZPortal.Core.Domain.Database.EntityTypes.Auditable;
using YZPortal.Worker.Helpers.Scriban;
using YZPortal.Worker.Infrastructure.Email;
using YZPortal.Worker.Infrastructure.Email.OfficeSmtp;
using YZPortal.Worker.Infrastructure.Email.SendGrid;
using YZPortal.Worker.Infrastructure.ScheduledTasks;

namespace YZPortal.Worker.Tasks.Email.Dealers
{
    public class SendDealerInvitesTask : EmailTask
    {
        public SendDealerInvitesTask(IServiceScopeFactory serviceScopeFactory, IOptions<ScheduledTasksOptions> options, IOptions<SendGridOptions> sendGridOptions, IOptions<EmailOptions> emailOptions, IOptions<OfficeSmtpOptions> officeSmtpOptions) : base(serviceScopeFactory, options, sendGridOptions, emailOptions, officeSmtpOptions)
        {
        }

        public override async Task ProcessInScope(IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            var dbContext = serviceProvider.GetRequiredService<PortalContext>();
            var logger = serviceProvider.GetRequiredService<ILogger<SendDealerInvitesTask>>();

            await SendInvitesAsync(dbContext, logger, cancellationToken);
        }

        public async Task SendInvitesAsync(PortalContext dbContext, ILogger<SendDealerInvitesTask> logger, CancellationToken cancellationToken)
        {
            logger.LogInformation("Sending invites ...");

            try
            {
                // Parse and validate template
                var template = ScribanHelper.ParseTemplate("Tasks/Email/Dealers/SendDealerInvite.html");
                if (template == null)
                    throw new Exception("Failed to parse template for " + nameof(SendDealerInvitesTask));

                // Query
                var lastAttempted = DateTime.UtcNow.Subtract(emailOptions.AttemptInterval);
                var dbInvites = await dbContext.DealerInvites
                    .Include(i => i.Dealer)
                    .Where(x =>
                        (x.SentDateTime == null && (x.ValidUntilDateTime > DateTime.UtcNow || x.ValidUntilDateTime == null)) || // (has not been sent AND is still valid) OR
                        (
                            (x.FailedSentDateTime != null && x.SentDateTime == null) &&// (it has failed AND
                            x.Attempts < emailOptions.SendAttempts && // has not been attempted too often AND                     
                            x.LastAttemptedSentDateTime < lastAttempted // enough time has passed since last attempt)
                        )
                    ).ToListAsync();

                // Prepare template input
                var models = dbInvites
                    .GroupBy(x => x.Email)
                    .Select(x => new SendInviteHtmlInput
                    {
                        callbackurl = x.ToList().Select(x => x.CallbackUrl).First(),
                        dealername = string.Join(", ", x.ToList().Select(x => x.Dealer.Name).ToList()),
                        Notifications = x.ToList()
                    }).ToList();

                // Send notifications and update emailable entities
                var result = await SendNotificationsAsync(models.Select(x => new EmailMessage
                {
                    Content = template.Render(x),
                    Subject = "YZ Portal Invite",
                    Notifications = x.Notifications
                }).ToList(), cancellationToken);

                await dbContext.BulkUpdateAsync(dbInvites, cancellationToken: cancellationToken);

                logger.LogInformation($"Invites sent: ({result})");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error while sending invites: {ex.Message}");
            }
        }

        public class SendInviteHtmlInput
        {
            public string? callbackurl { get; set; }
            public string? dealername { get; set; }
            public IEnumerable<EmailableEntity> Notifications { get; set; } = new List<EmailableEntity>();
        }
    }
}

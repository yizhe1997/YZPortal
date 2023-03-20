using Microsoft.Extensions.Options;
using NCrontab;
using SendGrid.Core;
using System.Net;
using System.Net.Mail;
using YZPortal.Worker.Infrastructure.Email;
using YZPortal.Worker.Infrastructure.Email.OfficeSmtp;
using YZPortal.Worker.Infrastructure.Email.SendGrid;
using YZPortal.Worker.Infrastructure.ScheduledTasks;

namespace YZPortal.Worker.Tasks.Email
{
    public abstract class EmailTask : ScheduledTask
    {
        protected override CrontabSchedule Schedule => CrontabSchedule.Parse(_options.EmailSchedule);
        protected readonly SendGridOptions sendGridOptions;
        protected readonly EmailOptions emailOptions;
        protected readonly OfficeSmtpOptions officeSmtpOptions;

        protected EmailTask(IServiceScopeFactory serviceScopeFactory, IOptions<ScheduledTasksOptions> options, IOptions<SendGridOptions> sendGridOptions, IOptions<EmailOptions> mailOptions, IOptions<OfficeSmtpOptions> officeSmtpOptions) : base(serviceScopeFactory, options)
        {
            this.sendGridOptions = sendGridOptions.Value;
            this.officeSmtpOptions = officeSmtpOptions.Value;
            emailOptions = mailOptions.Value;
        }

        public async Task<SendNotificationResult> SendNotificationsAsync(List<EmailMessage> messages, CancellationToken cancellationToken)
        {
            var failedCount = 0;
            var sentCount = 0;

            foreach (var message in messages)
            {
                try
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }

                    switch (emailOptions.Client)
                    {
                        case (int)EmailClientNames.None:
                            message.UpdateNotificationsFailed("Email client service is turned off", true);

                            break;

                        case (int)EmailClientNames.SendGrid:
                            var sendGridMessage = message.ConstructSendGridMessage(sendGridOptions);
                            var status = await SendGridServer.SendMailAsync(sendGridMessage);

                            if (status.ToString() != HttpStatusCode.OK.ToString() && status.ToString() != HttpStatusCode.Accepted.ToString())
                            {
                                message.UpdateNotificationsFailed($"Error communicating with SendGridApi: HttpStatus {status}");
                                failedCount++;
                            }
                            else
                            {
                                message.UpdateNotificationsPassed();
                                sentCount++;
                            }
                            break;

                        case (int)EmailClientNames.OfficeSmtp:

                            SmtpClient SmtpClient = officeSmtpOptions.ConstructOfficeSmtpMessage();

                            MailMessage officeSmtpMessage = new MailMessage(officeSmtpOptions.SenderEmail, message.Notifications.Select(x => x.Email).First(), message.Subject, message.Content);
                            officeSmtpMessage.IsBodyHtml = true;

                            await SmtpClient.SendMailAsync(officeSmtpMessage);

                            message.UpdateNotificationsPassed();
                            sentCount++;
                            break;

                        default:
                            message.UpdateNotificationsFailed("Email client option index out of range", true);
                            
                            break;
                    }
                }
                catch (Exception ex)
                {
                    message.UpdateNotificationsFailed(ex.ToString());
                    failedCount++;
                }
            }

            return new SendNotificationResult { Sent = sentCount, Failed = failedCount };
        }

        public class SendNotificationResult
        {
            public int Sent { get; set; }
            public int Failed { get; set; }

            public override string ToString()
            {
                return $"Sent: {Sent}, Failed: {Failed}";
            }
        }
    }
}

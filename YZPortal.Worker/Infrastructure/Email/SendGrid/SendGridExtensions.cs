using SendGrid.Core;
using YZPortal.Worker.Tasks.Email;

namespace YZPortal.Worker.Infrastructure.Email.SendGrid
{
    public static class SendGridExtensions
    {
        public static SendGridMailMessage ConstructSendGridMessage(this EmailMessage message, SendGridOptions sendGridOptions)
        {
            return new SendGridMailMessage
            {
                Content = new List<MessageContent>
                {
                    {
                        new MessageContent
                        {
                            Type = "text/html",
                            Value = message.Content
                        }
                    }
                },
                From = new SendGridMailContact
                {
                    Email = sendGridOptions.SenderEmail,
                    Name = sendGridOptions.SenderName ?? sendGridOptions.SenderEmail
                },
                Personalizations = new List<Personalization>
                {
                    new Personalization
                    {
                        To = new List<SendGridMailContact>
                        {
                            new SendGridMailContact
                            {
                                Email = message.Notifications.Select(x => x.Email).First()
                            }
                        }
                    }
                },
                Subject = message.Subject
            };
        }
    }
}

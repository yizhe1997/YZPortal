using Application.Interfaces.Services.Mailing;
using Application.Requests;
using Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;

namespace Infrastructure.Services.Mailing
{
    public class GoogleMailService : IMailService
    {
        private readonly MailConfig _mailConfig;

        public GoogleMailService(IOptions<MailConfig> mailConfig)
        {
            _mailConfig = mailConfig.Value;
        }

        public async Task SendAsync(CreateMailCommand request)
        {
            var smtpClient = new SmtpClient(_mailConfig.GoogleSMTP.HostName)
            {
                EnableSsl = _mailConfig.GoogleSMTP.EnableSSL,
                UseDefaultCredentials = false,
                Port = _mailConfig.GoogleSMTP.Port,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(_mailConfig.GoogleSMTP.NetworkCredUsername, _mailConfig.GoogleSMTP.NetworkCredPassword) 
            };

            var mailMessage = new MailMessage
            {
                Subject = request.Subject,
                Body = request.HtmlContent,
                IsBodyHtml = true,
                From = new MailAddress(_mailConfig.GoogleSMTP.FromEmail, _mailConfig.GoogleSMTP.FromName),
            };

            // Reply-Tos
            request.ReplyTos.ForEach(x =>
            {
                mailMessage.ReplyToList.Add(new MailAddress(x.Email, x.Name));
            });

            mailMessage.ReplyToList.Add(new MailAddress(_mailConfig.GoogleSMTP.FromEmail, _mailConfig.GoogleSMTP.FromName));

            // Attachments
            request.Attachments.ForEach(x =>
            {
                mailMessage.Attachments.Add(new Attachment(x.OpenReadStream(), x.FileName, x.ContentType));
            });

            // Tos
            request.Tos.ForEach(x =>
            {
                mailMessage.To.Add(new MailAddress(x.Email, x.Name));
            });

            // Bccs
            request.Bccs.ForEach(x =>
            {
                mailMessage.Bcc.Add(new MailAddress(x.Email, x.Name));
            });

            // Ccs
            request.Ccs.ForEach(x =>
            {
                mailMessage.CC.Add(new MailAddress(x.Email, x.Name));
            });

            try
            {
                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
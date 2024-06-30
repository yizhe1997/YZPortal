using Application.Interfaces.Services.Mailing;
using Application.Requests;
using Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;

namespace Infrastructure.Services.Mailing
{
    public class GoogleMailService(IOptions<MailConfig> mailConfig) : IMailService
    {
        public async Task SendAsync(CreateMailCommand request)
        {
            var smtpClient = new SmtpClient(mailConfig.Value.GoogleSMTP.HostName)
            {
                EnableSsl = mailConfig.Value.GoogleSMTP.EnableSSL,
                UseDefaultCredentials = false,
                Port = mailConfig.Value.GoogleSMTP.Port,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(mailConfig.Value.GoogleSMTP.NetworkCredUsername, mailConfig.Value.GoogleSMTP.NetworkCredPassword) 
            };

            var mailMessage = new MailMessage
            {
                Subject = request.Subject,
                Body = request.HtmlContent,
                IsBodyHtml = true,
                From = new MailAddress(mailConfig.Value.GoogleSMTP.FromEmail, mailConfig.Value.GoogleSMTP.FromName),
            };

            // Reply-Tos
            request.ReplyTos.ForEach(x =>
            {
                mailMessage.ReplyToList.Add(new MailAddress(x.Email, x.Name));
            });

            mailMessage.ReplyToList.Add(new MailAddress(mailConfig.Value.GoogleSMTP.FromEmail, mailConfig.Value.GoogleSMTP.FromName));

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
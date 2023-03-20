using System.Net.Mail;
using System.Net;

namespace YZPortal.Worker.Infrastructure.Email.OfficeSmtp
{
    public static class OfficeSmtpExtensions
    {
        public static SmtpClient ConstructOfficeSmtpMessage(this OfficeSmtpOptions officeSmtpOptions)
        {
            SmtpClient SmtpClient = new SmtpClient();
            SmtpClient.Credentials = new NetworkCredential(officeSmtpOptions.SenderEmail, officeSmtpOptions.Password);
            SmtpClient.Host = officeSmtpOptions.Host;
            SmtpClient.Port = officeSmtpOptions.Port;
            SmtpClient.EnableSsl = officeSmtpOptions.EnableSSL;

            return SmtpClient;
        }
    }
}

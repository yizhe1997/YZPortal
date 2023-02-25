namespace YZPortal.Worker.Infrastructure.Email.OfficeSmtp
{
    public class OfficeSmtpOptions
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSSL { get; set; }
        public string SenderEmail { get; set; }
        public string Password { get; set; }
    }
}

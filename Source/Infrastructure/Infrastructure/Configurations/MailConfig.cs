namespace Infrastructure.Configurations
{
    public class MailConfig
    {
        public GoogleSMTPConfig GoogleSMTP { get; set; } = new();
        public SendGridSMTPConfig SendGridSMTP { get; set; } = new();

        public abstract class SMTPConfig
        {
            public string? FromName { get; set; }
            public string? FromEmail { get; set; }
        }

        public class SendGridSMTPConfig : SMTPConfig
        {
            public string? ApiKey { get; set; }
        }

        public class GoogleSMTPConfig : SMTPConfig
        {
            public string? NetworkCredUsername { get; set; }
            public string? HostName { get; set; }
            public string? NetworkCredPassword { get; set; }
            public int Port { get; set; }
            public bool EnableSSL { get; set; }
        }
    }
}

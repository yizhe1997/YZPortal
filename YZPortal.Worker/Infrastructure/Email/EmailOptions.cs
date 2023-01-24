namespace YZPortal.Worker.Infrastructure.Email
{
    public class EmailOptions
    {
        public string WebServiceUrl { get; set; } = "http://example.com";
        public string WebServiceSecret { get; set; }
        public string Client { get; set; } = "WebService";
        public int SendAttempts { get; set; } = 3;
        public TimeSpan AttemptInterval { get; set; } = TimeSpan.FromHours(1);
    }
}

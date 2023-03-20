namespace YZPortal.Worker.Infrastructure.Email
{
    public class EmailOptions
    {
        public int Client { get; set; } = (int)EmailClientNames.None;
        public int SendAttempts { get; set; } = 3;
        public TimeSpan AttemptInterval { get; set; } = TimeSpan.FromHours(1);
    }

    [Flags]
    public enum EmailClientNames
    {
        None = 0,
        SendGrid = 1,
        OfficeSmtp = 2
    }
}

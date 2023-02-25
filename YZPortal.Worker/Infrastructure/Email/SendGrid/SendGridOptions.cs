namespace YZPortal.Worker.Infrastructure.Email.SendGrid
{
	public class SendGridOptions
	{
		public string ApiKey { get; set; }
		public string SenderEmail { get; set; }
		public string SenderName { get; set; }
	}
}

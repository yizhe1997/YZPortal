namespace YZPortal.FullStackCore.Models.Abstracts
{
    public class EmailableModel : AuditableModel
	{
		public string? Email { get; set; }
		public DateTime? SentDateTime { get; set; }
		public string? FailedMessage { get; set; }
		public DateTime? FailedSentDateTime { get; set; }
		public int Attempts { get; set; }
		public DateTime? LastAttemptedSentDateTime { get; set; }
	}
}

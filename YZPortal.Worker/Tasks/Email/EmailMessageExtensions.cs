namespace YZPortal.Worker.Tasks.Email
{
    public static class EmailMessageExtensions
    {
        public static void UpdateNotificationsFailed(this EmailMessage message, string? failedMessage = null, bool bypassAttemptUpdate = false)
        {
            foreach (var notification in message.Notifications)
            {
                notification.FailedSentDateTime = notification.LastAttemptedSentDateTime = DateTime.UtcNow;
                notification.FailedMessage = failedMessage;

                if(bypassAttemptUpdate == false)
                    notification.Attempts += 1;
            }
        }

        public static void UpdateNotificationsPassed(this EmailMessage message)
        {
            foreach (var notification in message.Notifications)
            {
                notification.SentDateTime = notification.LastAttemptedSentDateTime = DateTime.UtcNow;
                notification.FailedSentDateTime = null;
                notification.FailedMessage = null;
                notification.Attempts += 1;
            }
        }
    }
}

namespace Application.Models
{
    public class ChatPaylodModel
    {
        public string? Username { get; set; }
        public string? Message { get; set; }
        public bool IsMine { get; set; }
        public bool IsNotice { get; set; }
        public string? CSS => IsMine ? "sent" : "received";
    }
}

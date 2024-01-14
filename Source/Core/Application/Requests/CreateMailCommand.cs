using Application.Models;
using Microsoft.AspNetCore.Http;

namespace Application.Requests
{
    public class CreateMailCommand
    {
        public string? Subject { get; set; }

        public string? HtmlContent { get; set; }
        public List<MailModel> Tos { get; set; } = new();
        public List<MailModel> ReplyTos { get; set; } = new();
        public List<MailModel> Ccs { get; set; } = new();
        public List<MailModel> Bccs { get; set; } = new();
        public List<IFormFile> Attachments { get; set; } = new();

        // TODO:
        // Key = email, Value = name
        //public Dictionary<string, string> Tos { get; set; } = new Dictionary<string, string>();

        // Key = email, Value = name
        //public IDictionary<string, string> Bccs { get; set; } = new Dictionary<string, string>();

        // Key = email, Value = name
        //public Dictionary<string, string> Ccs { get; set; } = new Dictionary<string, string>();

        //// Key = name, Value = byte
        //public IDictionary<string, byte[]> Attachments { get; set; } = new Dictionary<string, byte[]>();

        //public IDictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
    }
}

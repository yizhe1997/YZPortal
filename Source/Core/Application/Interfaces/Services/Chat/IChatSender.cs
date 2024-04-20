using Application.Models;

namespace Application.Interfaces.Services.Chat
{
    public interface IChatSender
    {
        Task BroadcastToGroupAsync(ChatPaylodModel model);
    }
}

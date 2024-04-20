using Application.Constants;
using Application.Interfaces.Services.Chat;
using Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;

namespace Infrastructure.Services
{
    [Authorize(AuthenticationSchemes = Constants.AzureAdB2C)]
    [RequiredScope(ScopeConstants.APIAccess)]
    public class ChatHubService : Hub, IChatSender
    {
        public ChatHubService()
        {
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }

        // TODO: try with https://github.com/IntelliTect/IntelliTect.AspNetCore.SignalR.SqlServer
        // TODO: scaleout https://learn.microsoft.com/en-us/aspnet/signalr/overview/performance/scaleout-with-sql-server
        // TODO: make it group based
        // TODO: handle cts token
        public async Task BroadcastToGroupAsync(ChatPaylodModel model)
        {
            await Clients.All
            .SendAsync(ChatHubConstants.TransferMessage, model);
        }
    }
}

using Microsoft.AspNetCore.SignalR;
using UpdatedChatApp.Interfaces;

namespace UpdatedChatApp.Helper
{
    public class SignalRHelper : Hub
    {
        private readonly IChatService chatService;
        private static Dictionary<string, string> ConnectedUsers = new Dictionary<string, string>();

        public SignalRHelper(IChatService chatService)
        {
            this.chatService = chatService;
        }

        public override async Task OnConnectedAsync()
        {
            var userId = Context.GetHttpContext().Request.Query["userId"].ToString();
            if (!string.IsNullOrEmpty(userId))
            {
                ConnectedUsers[userId] = Context.ConnectionId;
                await base.OnConnectedAsync();
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userId = ConnectedUsers.FirstOrDefault(x => x.Value == Context.ConnectionId).Key;
            if (!string.IsNullOrEmpty(userId))
            {
                ConnectedUsers.Remove(userId);
            }
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(Guid senderId, Guid receiverId, string message)
        {
            var savedMessage = await chatService.SaveMessageAsync(senderId, receiverId, message);

            // Send to receiver if online
            var receiverConnectionId = ConnectedUsers.GetValueOrDefault(receiverId.ToString());
            if (!string.IsNullOrEmpty(receiverConnectionId))
            {
                await Clients.Client(receiverConnectionId).SendAsync("ReceiveMessage", senderId, message, savedMessage.Timestamp);
            }

            // Send back to sender
            var senderConnectionId = ConnectedUsers.GetValueOrDefault(senderId.ToString());
            if (!string.IsNullOrEmpty(senderConnectionId))
            {
                await Clients.Client(senderConnectionId).SendAsync("ReceiveMessage", senderId, message, savedMessage.Timestamp);
            }
        }
    }
}

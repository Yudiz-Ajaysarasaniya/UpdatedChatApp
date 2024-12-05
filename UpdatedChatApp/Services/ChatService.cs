using Microsoft.EntityFrameworkCore;
using UpdatedChatApp.AppContext;
using UpdatedChatApp.Interfaces;
using UpdatedChatApp.model.Entities;

namespace UpdatedChatApp.Services
{
    public class ChatService : IChatService
    {
        private readonly AppDbContext dbContext;

        public ChatService(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<ChatMessage>> GetChatHistoryAsync(Guid user1Id, Guid user2Id)
        {
            return await dbContext.Messages
                 .Where(m => (m.SenderId == user1Id && m.ReceiverId == user2Id) ||
                             (m.SenderId == user2Id && m.ReceiverId == user1Id))
                 .OrderBy(m => m.Timestamp)
                 .ToListAsync();
        }

        public async Task<int> MessageCount(Guid senderId, Guid receiverId)
        {
            var count = dbContext.Messages.Count(x => x.SenderId.Equals(senderId) && x.ReceiverId.Equals(receiverId) && !x.IsRead);

            return count;
        }

        public async Task<ChatMessage> SaveMessageAsync(Guid senderId, Guid receiverId, string content)
        {
            var message = new ChatMessage
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Content = content,
                Timestamp = DateTime.UtcNow,
                IsRead = false
            };

            dbContext.Messages.Add(message);
            await dbContext.SaveChangesAsync();

            return message;
        }
    }
}

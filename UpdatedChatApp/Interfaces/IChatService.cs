using ChatApp.model.Entities;

namespace UpdatedChatApp.Interfaces
{
    public interface IChatService
    {
        Task<ChatMessage> SaveMessageAsync(Guid senderId, Guid receiverId, string content);
        Task<List<ChatMessage>> GetChatHistoryAsync(Guid user1Id, Guid user2Id);
    }
}

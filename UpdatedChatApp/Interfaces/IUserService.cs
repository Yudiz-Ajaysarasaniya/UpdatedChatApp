
using UpdatedChatApp.model.Request.Account;
using UpdatedChatApp.model.Response.User;

namespace UpdatedChatApp.Interfaces
{
    public interface IUserService
    {
        Task<List<UserListRequest>> GetUsers(string sender);
        Task<RegisterRequest> GetUserByUserId(Guid id);
    }
}

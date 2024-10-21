using ChatApp.model.Response.User;

namespace UpdatedChatApp.Interfaces
{
    public interface IUserService
    {
        Task<List<UserListRequest>> GetUsers(string sender);
    }
}

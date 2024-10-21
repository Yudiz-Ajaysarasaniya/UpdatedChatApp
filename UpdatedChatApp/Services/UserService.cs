using ChatApp.model.Response.User;
using Microsoft.EntityFrameworkCore;
using UpdatedChatApp.AppContext;
using UpdatedChatApp.Interfaces;

namespace UpdatedChatApp.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext appContext;

        public UserService(AppDbContext appContext)
        {
            this.appContext = appContext;
        }
        public async Task<List<UserListRequest>> GetUsers(string sender)
        {
            var users = await appContext.Users
                .Where(x => x.Email != sender)
                .Select(u => new UserListRequest
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email
                })
                .ToListAsync();

            return users;
        }
    }
}

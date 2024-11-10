using Microsoft.EntityFrameworkCore;
using UpdatedChatApp.AppContext;
using UpdatedChatApp.Interfaces;
using UpdatedChatApp.model.Entities;
using UpdatedChatApp.model.Request.Account;
using UpdatedChatApp.model.Response.User;

namespace UpdatedChatApp.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext appContext;

        public UserService(AppDbContext appContext)
        {
            this.appContext = appContext;
        }

        public async Task<RegisterRequest> GetUserByUserId(Guid id)
        {
            var user = await appContext.Users.FindAsync(id);
            if (user == null)
            {
                return null;
            }
            return new RegisterRequest
            {
                UserName = user.UserName,  
                Email = user.Email
                //Password = user.Password
            };
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

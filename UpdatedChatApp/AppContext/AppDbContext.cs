using ChatApp.model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System.Collections.Generic;

namespace UpdatedChatApp.AppContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Register> Users { get; set; }
        public DbSet<ChatMessage> Messages { get; set; }
    }
}

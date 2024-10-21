using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdatedChatApp.model.Request.Account
{
    public class LoginRequest
    {
        //public int Id { get; set; }//
        public string? Email { get; set; }
        public string? Password { get; set; }
        // public string? UniqueId { get; set; }
    }
}

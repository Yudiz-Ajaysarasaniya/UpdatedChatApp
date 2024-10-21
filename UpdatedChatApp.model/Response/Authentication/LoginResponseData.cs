using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdatedChatApp.model.Response.Authentication
{
    public class LoginResponseData
    {
        public Guid Id { get; set; }
        public string? Email { get; set; }
        public string? Token { get; set; }
    }
}

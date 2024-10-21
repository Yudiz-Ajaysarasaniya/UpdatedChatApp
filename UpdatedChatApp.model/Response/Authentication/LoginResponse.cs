using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdatedChatApp.model.Response.Authentication
{
    public class LoginResponse
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
        public LoginResponseData Data { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdatedChatApp.notify
{
    public interface IMessages : IDisposable
    {
        Task SendMail(string email, string subject, string body);
    }
}

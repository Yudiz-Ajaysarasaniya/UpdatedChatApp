using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UpdatedChatApp.notify.Model;

namespace UpdatedChatApp.notify.Implementation
{
    public class Messages : IMessages
    {
        private EmailConfig EmailConfig { get; set; }

        public Messages(IOptions<EmailConfig> options) => this.EmailConfig = options.Value;

        public async Task SendMail(string email, string subject, string body)
        {
            using SendEmailViaSmtp smtp = new(EmailConfig);
            smtp.Send(email, subject, body);
        }

        public void Dispose() => GC.SuppressFinalize(this);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdatedChatApp.notify.Model
{
    public class EmailConfig
    {
        public string? SmtpAddress { get; set; }

        public int Port { get; set; }

        public string? DisplayName { get; set; }

        public string? SenderEmail { get; set; }

        public string? Password { get; set; }

        // public string? SenderName { get; set; }

        public string? LoginUrl { get; set; }
    }
}

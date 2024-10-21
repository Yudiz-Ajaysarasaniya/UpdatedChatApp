using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UpdatedChatApp.notify.Model;

namespace UpdatedChatApp.notify.Implementation
{
    public class SendEmailViaSmtp : IDisposable
    {
        private EmailConfig config;
        public SendEmailViaSmtp(EmailConfig configs) => this.config = configs;

        #region Send

        public void Send(string receiverEmail, string subject, string body)
        {
            if (string.IsNullOrEmpty(receiverEmail)) throw new Exception($"Email con not be null");
            if (string.IsNullOrEmpty(config.SenderEmail)) throw new Exception($"Please specify the Sender Email in AppSettings.");
            if (string.IsNullOrEmpty(config.SmtpAddress)) throw new Exception($"Please specify the SMTP Address in AppSettings.");
            if (string.IsNullOrEmpty(config.Password)) throw new Exception($"Please specify the Password in AppSettings.");
            if (string.IsNullOrEmpty(config.Port.ToString())) throw new Exception($"Please specify the Port in AppSettings.");

            try
            {
                using var mail = new MailMessage
                {
                    From = new MailAddress(config.SenderEmail, config.DisplayName)
                };

                // send mail

                mail.To.Add(receiverEmail);
                mail.Subject = subject;
                mail.Body = body;

                using SmtpClient smtp = new SmtpClient(config.SmtpAddress, config.Port);
                smtp.Credentials = new NetworkCredential(config.SenderEmail, config.Password);
                smtp.EnableSsl = true;
                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.ToString());
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing) config = null;
        }

        #endregion
    }
}

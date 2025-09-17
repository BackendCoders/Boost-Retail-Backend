using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;
using Boost.Retail.Services.Interfaces;

namespace Boost.Retail.Services
{
    
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _config;

        public EmailSender(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var smtpClient = new SmtpClient(_config["EmailSettings:SMTPHost"])
            {
                Port = int.Parse(_config["EmailSettings:SMTPPort"]),
                Credentials = new NetworkCredential(_config["EmailSettings:SMTPUser"], _config["EmailSettings:SMTPPassword"]),
                EnableSsl = false
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_config["EmailSettings:FromEmail"]),
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            };
            mailMessage.To.Add(email);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}

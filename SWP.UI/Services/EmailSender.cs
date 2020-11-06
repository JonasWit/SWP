using Microsoft.AspNetCore.Identity.UI.Services;
using NETCore.MailKit.Core;
using System.Threading.Tasks;

namespace SWP.UI.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IEmailService emailService;
        public EmailSender(IEmailService emailService) => this.emailService = emailService;

        public Task SendEmailAsync(string email, string subject, string message) => Execute(subject, message, email);

        public Task Execute(string subject, string message, string email) => emailService.SendAsync(email, subject, message, true);
    }
}

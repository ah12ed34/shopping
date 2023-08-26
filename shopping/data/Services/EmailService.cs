using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Threading.Tasks;

namespace shopping.data.Services
{
    public interface IEmailService
    {
        Task SendAsync(string email, string subject, string message);
    }
    public class EmailService : IEmailService
    {
        //private readonly EmailSettings _emailSettings;

        //public EmailService(IOptions<EmailSettings> emailSettings)
        //{
        //    _emailSettings = emailSettings.Value;
        //}
        public Task SendAsync(string email, string subject, string message)
        {
            throw new NotImplementedException();
        }
    }
}

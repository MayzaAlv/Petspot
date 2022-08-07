using MimeKit;
using Petspot.Models;

namespace Petspot.Services
{
    public interface IEmailService
    {
        void SendEmail(string[] recipient, string subject,
            string[] usernames, string url);
    }
}

using MailKit.Net.Smtp;
using MimeKit;
using Petspot.Models;

namespace Petspot.Services
{
    public class EmailService
    {
        private IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendEmail(string[] recipient, string subject,
            string[] usernames, string url)
        {
            Message message = new Message(recipient,
                subject, usernames, url);

            var messageEmail = CreateEmailBody(message);
            Send(messageEmail);
        }

        private void Send(MimeMessage messageEmail)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_configuration.GetValue<string>("EmailSettings:SmtpServer"),
                                   _configuration.GetValue<int>("EmailSettings:Port"), false);
                    client.AuthenticationMechanisms.Remove("XOUATH2");
                    client.Authenticate(_configuration.GetValue<string>("EmailSettings:From"),
                                        _configuration.GetValue<string>("EmailSettings:Password"));
                    client.Send(messageEmail);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }

        private MimeMessage CreateEmailBody(Message message)
        {
            var messageEmail = new MimeMessage();
            messageEmail.From.Add(new MailboxAddress("Petspot",
                _configuration.GetValue<string>("EmailSettings:From")));
            messageEmail.To.AddRange(message.Recipient);
            messageEmail.Subject = message.Subject;
            messageEmail.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message.Content
            };
            return messageEmail;
        }
    }
}

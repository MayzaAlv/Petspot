using MimeKit;

namespace Petspot.Models
{
    public class Message
    {
        public List<MailboxAddress> Recipient { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }

        public Message(IEnumerable<string> recipient, string subject,
             string[] usernames, string content)
        {
            Recipient = new List<MailboxAddress>();
            for (int i = 0; i < usernames.Count(); i++)
            {
                MailboxAddress mba = new MailboxAddress(usernames[i], recipient.ToList()[i]);
                Recipient.Add(mba);
            }
            Subject = subject;
            Content = content;
        }
    }
}

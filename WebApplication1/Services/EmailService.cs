using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using WebApplication1.Configurations;

namespace WebApplication1.Services
{
    public  class EmailService
    {

        // private MimeMessage _message;
        // private MailboxAddress _address;

        // private SmtpClient _client;

        private readonly EmailSettings _es;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _es = emailSettings.Value;
        }

        public void Send(string toEmail, string subject, string body)
        {

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Jermaine Forbes", _es.Username));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = subject;

            message.Body = new TextPart("plain") { Text = body };

            using (var client = new SmtpClient())
            {
                client.Connect(_es.SmtpServer!, _es.Port, false);
                client.Authenticate(_es.Username, _es.Password);
                client.Send(message);
                client.Disconnect(true);
            }
        }

    }



}
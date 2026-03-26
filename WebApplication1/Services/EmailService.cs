using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using WebApplication1.Configurations;
using System.Reflection;
using System.Web;

namespace WebApplication1.Services
{
    public class EmailService
    {

        // private MimeMessage _message;
        // private MailboxAddress _address;

        // private SmtpClient _client;

        private readonly EmailSettings _es;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _es = emailSettings.Value;
        }

        public void Send<T>(ref T emailObject, string template, string toEmail)
        {

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Jermaine Forbes", _es.Username));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = emaliObject.Title;

            var body = PopulateBody(emailObject, template);

            message.Body = new TextPart("plain") { Text = body };

            using (var client = new SmtpClient())
            {
                client.Connect(_es.SmtpServer!, _es.Port, false);
                client.Authenticate(_es.Username, _es.Password);
                client.Send(message);
                client.Disconnect(true);
            }
        }

        private string PopulateBody(dynamic emailObject, string template)
        {
            string body = string.Empty;
            Type type = emailObject.GetType();
            using (StreamReader reader = new StreamReader(Server.MapPath($"~/Templates/{template}.html")))
            {
                body = reader.ReadToEnd();
            }

            foreach (PropertyInfo property in type.GetProperties())
            {
                if(body.Contains(property.Name))
                {
                    body = body.Replace("{"+property.Name+"}", property.GetValue(emailObject));
                }
                // Console.WriteLine($"{property.Name}: {property.GetValue(emailObject)}");
            }

            return body;
        }

    }



}
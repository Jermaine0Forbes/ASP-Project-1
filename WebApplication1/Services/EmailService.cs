using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using WebApplication1.Configurations;
using System.Reflection;
using System.Web;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Text;

namespace WebApplication1.Services
{
    public class EmailService
    {

        // private MimeMessage _message;
        // private MailboxAddress _address;

        // private SmtpClient _client;

        private readonly EmailSettings _es;
        private readonly IWebHostEnvironment _whe;

        public EmailService(IOptions<EmailSettings> emailSettings, IWebHostEnvironment webHostEnvironment)
        {
            _es = emailSettings.Value;
            _whe = webHostEnvironment;
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
            string templateFileName = $"{template}.html";
            string filePath = Path.Combine(_whe.ContentRootPath, "Templates", templateFileName);
            try
            {
                

                using (StreamReader reader = new StreamReader(filePath, Encoding.UTF8))
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
            } catch (IOException e)
            {
                throw new Exception(e.Message);
            }

            return body;
        }

    }



}
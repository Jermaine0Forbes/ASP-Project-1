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
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using WebApplication1.Interfaces;
using System.Threading.Tasks;

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

        public void Send<T>( T emailObject, string template, string toEmail)  where T : IEmail
        {
            if(emailObject == null)
            {
                throw new Exception("bad news sending email");
            }

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Jermaine Forbes", _es.Username));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = emailObject?.Title ?? "";

            var body = PopulateBody(emailObject, template) ?? "";

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = body;

            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                client.Connect(_es.SmtpServer!, _es.Port, false);
                client.Authenticate(_es.Username, _es.Password);
                client.Send(message);
                client.Disconnect(true);
            }
        }

        private string PopulateBody<T>(T emailObject, string template) where T : IEmail
        {
            string body = string.Empty;
            try
            {
                
            Type type = emailObject.GetType();
            string templateFileName = $"{template}.html";
            string filePath = Path.Combine(_whe.ContentRootPath, "Templates", templateFileName);

                using (StreamReader reader = new StreamReader(filePath, Encoding.UTF8))
                {
                    body = reader.ReadToEnd();
                }

                foreach (PropertyInfo property in type.GetProperties())
                {
                    if(body.Contains(property.Name))
                    {
                        var name = "{"+property.Name+"}";
                        var value = property.GetValue(emailObject) ?? "";
                        body = body.Replace(name, value.ToString());
                    }
                }
            } catch (IOException e)
            {
                throw new Exception(e.Message);
            }

            return body;
        }

    }



}
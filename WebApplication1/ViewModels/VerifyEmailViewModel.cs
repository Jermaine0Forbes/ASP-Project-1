using System.ComponentModel.DataAnnotations;

namespace WebApplication1.ViewModels
{
    public class VerifyEmailViewModel
    {

        public string? userId { get; set; }

        public string? token { get; set;}
    }
}
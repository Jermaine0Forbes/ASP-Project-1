
using System.ComponentModel.DataAnnotations;
using WebApplication1.Interfaces;

namespace WebApplication1.Models
{
    

    public class ConfirmationEmailModel: IEmail
    {
        [Required]
        public string UserName {get; set;} = "";

        [Required]
        public string Title {get; set;} = "";

        public string Url {get; set;} = "";
    }
}
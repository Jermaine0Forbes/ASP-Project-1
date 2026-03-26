
using System.ComponentModel.DataAnnotations;
using WebApplication1.Interfaces;

namespace WebApplication1.Models
{
    

    public class DefaultEmailModel: IEmail
    {
        [Required]
        public string UserName {get; set;} = "";

        [Required]
        public string Title {get; set;} = "";

        [Required]
        public string Description {get; set;} = "";

        public string Url {get; set;} = "";
    }
}
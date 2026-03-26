
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    

    public class DefaultEmailModel
    {
        [Required]
        public string UserName {get; set;} = "";

        [Required]
        public string Title {get; set;} = "";

        [Required]
        public string Description {get; set;} = "";
    }
}
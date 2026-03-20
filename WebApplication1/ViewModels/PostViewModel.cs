using System.ComponentModel.DataAnnotations;

namespace WebApplication1.ViewModels
{
    public class PostViewModel
    {
        [Required(ErrorMessage = "Title is required!")]
        [StringLength(255, MinimumLength = 8, ErrorMessage = "The {0} must be at {2} and at max {1} characters long!")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Message is required!")]
        [Display(Name="Message")]
        [StringLength(500, MinimumLength = 3, ErrorMessage = "The {0} must be at {2} and at max {1} characters long!")]
        public string? Body { get; set; }
    }
}
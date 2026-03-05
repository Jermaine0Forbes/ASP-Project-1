using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models;

public class Post 
{

    public int Id { get; set; }

    [Required]
    [StringLength(255, MinimumLength =3)]
    public string Title { get; set; } = "";

    [Required]
    [Display(Name="Message")]
    [DataType(DataType.MultilineText)]
    public string Body {get; set;} = "";

    [Display(Name="Created At")]
    [DataType(DataType.Date)]
    public DateTime CreatedAt { get; set; }

    [Display(Name ="Updated At")]
    [DataType(DataType.Date)]
    public DateTime? UpdatedAt { get; set; }

    public User? User {get; set;}



}

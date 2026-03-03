using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models;

public class UserModel
{
    public int Id { get; set; }
    public string? Username { get; set; }

    public string? Email { get; set; }

    public string Password { get; set; } = "";

    [Display(Name="Created At")]
    [DataType(DataType.Date)]
    public DateTime CreatedAt { get; set; }
    [Display(Name ="Updated At")]
    [DataType(DataType.Date)]
    public DateTime? UpdatedAt { get; set; }



}

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Models;

public class UserModel : IdentityUser
{

    public string? Username { get; set; }

    public string Password { get; set; } = "";

    [Display(Name="Created At")]
    [DataType(DataType.Date)]
    public DateTime CreatedAt { get; set; }

    [Display(Name ="Updated At")]
    [DataType(DataType.Date)]
    public DateTime? UpdatedAt { get; set; }



}

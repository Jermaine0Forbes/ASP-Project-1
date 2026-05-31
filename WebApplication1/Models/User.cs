using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Models;

public class User : IdentityUser
{


    [Display(Name="Created At")]
    [DataType(DataType.Date)]
    public DateTime CreatedAt { get; set; }

    [Display(Name ="Updated At")]
    [DataType(DataType.Date)]
    public DateTime? UpdatedAt { get; set; }

    [Display(Name="Profile Image")]
    public String? Image {get; set;}

    // [Display(Name ="Password")]
    // public new string? PasswordHash {get; set;} 

    [DataType(DataType.Date)]
    public DateTime? OtpExpirationDate {get; set;}

    public ICollection<Post> Posts {get; set;} = [];

    [Display(Name="Roles")]
    public virtual ICollection<UserRole> UserRoles {get; set;} = [];


}

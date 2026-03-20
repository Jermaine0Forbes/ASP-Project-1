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

    // [Display(Name="Username")]
    // public new string UserName {get; set;} = "";



}

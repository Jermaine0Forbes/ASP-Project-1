using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebApp2.Models;


public class User:IdentityUser
{
    [Display(Name="Created At")]
    [DataType(DataType.Date)]
    public DateTime CreatedAt { get; set; }

    [Display(Name ="Updated At")]
    [DataType(DataType.Date)]
    public DateTime? UpdatedAt { get; set; }
}
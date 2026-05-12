using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication1.Models;

namespace WebApplication1.ViewModels
{
    public class UserCreateViewModel
    {

         public string? Role { get; set; }
         public User? User{ get; set; }


         public void Deconstruct(out User theUser, out string theRole)
        {
            theUser = User;
            theRole = Role;
        }
        

    }
}
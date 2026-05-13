using System.ComponentModel.DataAnnotations;
using WebApplication1.Models;

namespace WebApplication1.ViewModels
{
    public class UserCreateViewModel
    {

         public string? Role { get; set; }
         public User? User{ get; set; }


         public void Deconstruct(out User theUser, out string theRole)
        {
            theUser = User ?? throw new Exception("User is null");
            theRole = Role ?? throw new Exception("Role is null");
        }
        

    }
}
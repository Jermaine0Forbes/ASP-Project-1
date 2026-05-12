using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication1.Models;

namespace WebApplication1.ViewModels
{
    public class UserEditViewModel
    {

         public string? Role { get; set; }
         public User? User{ get; set; }
        
        public List<SelectListItem>? Roles { get; set; }

    }
}
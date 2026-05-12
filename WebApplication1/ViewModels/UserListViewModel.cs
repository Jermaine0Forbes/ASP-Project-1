using System.ComponentModel.DataAnnotations;
using WebApplication1.Models;

namespace WebApplication1.ViewModels
{
    public class UserListViewModel
    {
        public string? Id {get; set;}
        public DateTime CreatedAt { get; set; } 

         public string? UserName { get; set; }
         public string? Email{ get; set; }
        
        public string? Roles { get; set; }

    }
}
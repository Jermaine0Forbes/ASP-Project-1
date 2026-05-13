using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication1.Models;

namespace WebApplication1.ViewModels
{
    public class AccountProfileViewModel
    {

         public User User{ get; set; } = new();

         public string? Image {get; set;}
        
        public List<Post>? Posts { get; set; } = [];

    }
}
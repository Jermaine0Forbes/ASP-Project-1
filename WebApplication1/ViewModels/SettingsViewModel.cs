using System.ComponentModel.DataAnnotations;
using WebApplication1.Models;

namespace WebApplication1.ViewModels
{
    public class SettingsViewModel
    {

         public ICollection<Settings>? Settings { get; set; }
         public Settings? Setting{ get; set; }


         public void Deconstruct(out Settings set, out ICollection<Settings> sets)
        {
            set = Setting ?? throw new Exception("Setting is null");
            sets = Settings ?? throw new Exception("Settings is nul");
        }
        

    }
}
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.ViewModels
{
    public class IpDailyViewModel
    {
        public int Hour { get; set; } 

         public string? Label { get; set; }
        
        public int Num { get; set; }

    }
}
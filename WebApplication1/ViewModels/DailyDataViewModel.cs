using System.ComponentModel.DataAnnotations;

namespace WebApplication1.ViewModels
{
    public class DailyDataViewModel
    {
        public int Hour { get; set; } 

         public string? Label { get; set; }
        
        public int Num { get; set; }

    }
}
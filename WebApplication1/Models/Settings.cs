using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models;

public class Settings
{

    public int Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength =3)]
    public string Key { get; set; } = "";

    [Required]
    public string Value {get; set;} = "";

    public int IsInt {get; set;} = 0;

    [Display(Name="Created At")]
    [DataType(DataType.Date)]
    public DateTime CreatedAt { get; set; }

}

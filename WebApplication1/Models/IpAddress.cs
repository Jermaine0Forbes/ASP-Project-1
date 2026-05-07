using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models;

public class IpAddress
{

    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Address { get; set; } = "";

    public string? UserId {get; set;}

    public string Latitude {get; set;} = "";
    public string Longitude {get; set;} = "";
    [Display(Name="Country Code")]
    public string? CountryCode {get; set;}
    [Display(Name="Country Name")]
    public string? CountryName {get; set;}
    public string? Region {get; set;}
    public string? City {get; set;}
    public string? Zip {get; set;}
    public string? Width {get; set;}
    public string? Height {get; set;}

    [Required]
    [Display(Name="Created At")]
    [DataType(DataType.Date)]
    public DateTime CreatedAt { get; set; }

}

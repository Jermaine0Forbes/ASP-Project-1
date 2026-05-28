using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Models;

public class UserRole : IdentityUserRole<string>
{

    public  virtual Role? Role {get; set;} 

    public virtual User? User {get; set;}

}


public class Role: IdentityRole<string>
{


    public Role(string name) 
    {
        Console.WriteLine($"{name} role is created");
        Name = name;
    }

    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public override string Id { get; set; }
    public virtual ICollection<UserRole> UserRoles {get; set;} = [];
}

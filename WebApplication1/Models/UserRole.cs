using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Models;

public class UserRole : IdentityUserRole<string>
{

    public  virtual Role? Role {get; set;} 

    public virtual User? User {get; set;}

}


public class Role: IdentityRole<string>
{
    public virtual ICollection<UserRole> UserRoles {get; set;} = [];
}

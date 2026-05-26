using Microsoft.AspNetCore.Identity;
using WebApplication1.Models;

namespace WebApplication1.Interfaces;

public interface IRolePermission
{
    public static readonly string RoleName;
    private static List<string> Permissions {get; set;} = [];
     private static List<string> Exclude {get; set;} = [];
    public string Can(string permission);


}
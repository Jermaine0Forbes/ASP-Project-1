using WebApplication1.Interfaces;
using System.Reflection;
using Microsoft.AspNetCore.Identity;
using WebApplication1.Models;
using System.Security.Claims;
namespace WebApplication1.Claims;

public static class RoleClaims
{

    public static RolePermission GetRolePermissions(string role)
    {
        return role.ToLower() switch
        {
            "admin" => new AdminPermission(),
            "manager" => new ManagerPermission(),
            _ => throw new Exception($"{role} permission does not exist")
        };

    }

    public static async Task AddClaims( string roleName, RoleManager<Role> roleM)
    {
        var rp = RoleClaims.GetRolePermissions(roleName);
        var r = await roleM.FindByIdAsync(rp.GetName()) ?? throw new Exception($"Cannot find {rp.GetName()} role");
        var claims = await roleM.GetClaimsAsync(r);
        foreach (var permission in rp.GetPermissions())
        {
            if (!claims.Any(x =>
                x.Type == "Permission" &&
                x.Value == permission)
            )
            {
                await roleM.AddClaimAsync(
                    r,
                    new Claim(
                        "Permission",
                        permission));
            }

        }
    }



}


public class AdminPermission : RolePermission
{
    public static new readonly string RoleName = "Admin";
    private static List<string> Exclude = [];
    private static List<string> Permissions = Permission.GetAllPermissions();
    public new string Can(string permission)
    {
        return permission switch
        {
            "delete user" => "CanDeleteUser",

            _ => throw new Exception($"{permission} , does not exist")
        };
    }


}


public class ManagerPermission : RolePermission
{
    public static new readonly string RoleName = "Manager";
    private static List<string> Exclude = new List<string> { "CanFlagUser" };
    private static List<string> Permissions = Permission.GetAllPermissions().Except(Exclude).ToList();
    public new string Can(string permission)
    {
        return permission switch
        {
            "edit user" => "CanEditUser",

            _ => throw new Exception($"{permission} , does not exist")
        };
    }

    public static async Task AddClaims(RoleManager<Role> roleM)
    {
        var r = await roleM.FindByIdAsync(RoleName) ?? throw new Exception($"Cannot find {RoleName} role");
        var claims = await roleM.GetClaimsAsync(r);
        foreach (var permission in Permissions)
        {
            if (!claims.Any(x =>
                x.Type == "Permission" &&
                x.Value == permission)
            )
            {
                await roleM.AddClaimAsync(
                    r,
                    new Claim(
                        "Permission",
                        permission));
            }

        }
    }
}

public class RolePermission : IRolePermission
{
    public static readonly string RoleName = "";
    private static List<string> Exclude =[];
    private static List<string> Permissions = [];

    public string Can(string permission)
    {
        return permission switch
        {
            "edit user" => "CanEditUser",

            _ => throw new Exception($"{permission} , does not exist")
        };
    }
    public string GetName()
    {
        return RoleName;
    }

    public  List<string> GetPermissions()
    {
        return Permissions;
    }
}

public static class Permission
{
    public static string CanEditUser = "CanEditUser";
    public static string CanAddUser = "CanAddUser";
    public static string CanDeleteUser = "CanDeleteUser";
    public static string CanFlagUser = "CanFlagUser";

    public static List<string> GetAllPermissions()
    {
        return [.. typeof(Permission).GetProperties().Select(p => p.Name)];
    }

}
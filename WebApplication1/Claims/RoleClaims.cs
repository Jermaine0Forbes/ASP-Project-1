using WebApplication1.Interfaces;
using System.Reflection;
namespace WebApplication1.Claims;

public static class RoleClaims
{
    
    public static IRolePermission GetPermissions(string role)
    {
        return role.ToLower() switch
        {
            "admin" => new AdminPermission(),
            "manager" => new ManagerPermission(),
            _ => throw new Exception($"{role} permission does not exist")
        };
        
    }



}


public  class AdminPermission : IRolePermission
{
    private static List<string> Exclude = [];
    private static List<string> Permissions = Permission.GetAllPermissions();
    public string Can(string permission)
    {
        return permission switch
        {
            "delete user" => "CanDeleteUser",

            _=> throw new Exception($"{permission} , does not exist")
        };
    }
}


public  class ManagerPermission : IRolePermission
{
    private static List<string> Exclude = new List<string> {"CanFlagUser"};
    private static List<string> Permissions = Permission.GetAllPermissions().Except(Exclude).ToList();
    public string Can(string permission)
    {
        return permission switch
        {
            "edit user" => "CanEditUser",

            _=> throw new Exception($"{permission} , does not exist")
        };
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
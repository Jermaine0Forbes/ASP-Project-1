using System.Text.Json.Nodes;
using System.Reflection;
using System.Text.Json;

namespace ConsoleApp1.Examples;

public  static class Permission
{
    public  static string CanEditUser = "CanEditUser";
    public static  string CanAddUser = "CanAddUser";
    public static  string CanDeleteUser = "CanDeleteUser";
    public static  string CanFlagUser = "CanFlagUser";

    public static List<string> GetAllPermissions()
    {
        var x = typeof(Permission)
        .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
        ;
        Console.WriteLine(x.Any());
        foreach( var y in x)
        {
            Console.WriteLine($"{y.Name}");
        }
        Console.WriteLine("get all permissions");
        Console.WriteLine(x.ToArray().Any());
        // Console.WriteLine(string.Join(",",x.ToArray()));
        return x.Select(p => p.Name).ToList();
    }

}


public class Jungle
{
    
    public string? Leopard {get; set;}
    public string? Pirannah {get; set;}
    public string? Sloth;
    public string? Monkey;

    public List<string> GetProps()
    {
        // var x = typeof(Jungle).GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
        // .Select(p => p.Name).ToList();
        var j = new Jungle();
        var x = j.GetType()
        .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
        .Select(p => p.Name).ToList();
        Console.WriteLine("get props");
        // Console.WriteLine( JsonSerializer.Serialize(j.GetType().GetProperties()));
        Console.WriteLine(string.Join(",", x));
        Console.WriteLine(string.Join(",", new List<string> {"x", "z", "y"}));
        return x;
    }

}
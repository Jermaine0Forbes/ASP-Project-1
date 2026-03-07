using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using WebApplication1.Data;
using System;
using System.Linq;
using Bogus;
using NuGet.Protocol;

namespace WebApplication1.Models;

public static class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        Random random = new();

        string[] roles = { "User", "Manager", "Admin" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        using var context = new AppDBContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<AppDBContext>>());
        // Look for any Users.
        if (context.Users.Any())
        {
            return;   // DB has been seeded
        }

        var fakeUsers = new Faker<User>()
        .RuleFor(u => u.UserName, f => f.Internet.UserName())
        .RuleFor(u => u.Email, f => f.Internet.Email())
        .RuleFor(u => u.PasswordHash, f => f.Internet.Password())
        .RuleFor(u => u.CreatedAt, f => f.Date.Soon())
        .RuleFor(u => u.UpdatedAt, f => f.Date.Soon())
        .FinishWith((f, u) =>
        {
            context.Posts.Add(new Post
            {
                Title = u.UserName + ": Title",
                Body = f.Lorem.Sentence(),
                CreatedAt = u.CreatedAt,
                User = u,
            });

        });
        var users = fakeUsers.Generate(10);

        // context.Users.AddRange(users);
        // context.SaveChanges();


        var r = roles[random.Next(0, 2)];
        foreach (var user in users)
        {
            var u = new User
            {
                UserName = user.UserName,
                Email = user.Email,
                PasswordHash = "Password123!",
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };
            var result = await userManager.CreateAsync(u, "Password123!");
            if (result.Succeeded)
            {
                r = roles[random.Next(0, 2)];
                await userManager.AddToRoleAsync(u, r);

            }
            else
            {
                
                throw new Exception(result.Errors.ToJson());
            }
        //   context.SaveChanges();

        }

    }

    //     public static void Initialize(IServiceProvider serviceProvider)
    // {
    //     //  var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    //     var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
    //     var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    //     Random random = new();

    //     // string[] roles = {"Admin", "Manager", "User"};
    //     // foreach(var role in roles)
    //     // {
    //     //     if(!await roleManager.RoleExistsAsync(role))
    //     //     {
    //     //         await roleManager.CreateAsync(new IdentityRole(role));
    //     //     }
    //     // }

    //     using var context = new AppDBContext(
    //         serviceProvider.GetRequiredService<
    //             DbContextOptions<AppDBContext>>());
    //     // Look for any Users.
    //     if (context.Users.Any())
    //     {
    //         return;   // DB has been seeded
    //     }

    //     var fakeUsers = new Faker<User>()
    //     .RuleFor(u => u.UserName, f => f.Internet.UserName())
    //     .RuleFor(u => u.Email, f => f.Internet.Email())
    //     .RuleFor(u => u.PasswordHash, f => f.Internet.Password())
    //     .RuleFor(u => u.CreatedAt, f => f.Date.Soon())
    //     .RuleFor(u => u.UpdatedAt, f => f.Date.Soon());
    //     var users = fakeUsers.Generate(10);

    //     Console.WriteLine(users.ToJson());

    //     // var r = roles[random.Next(0, 2)];
    //     // foreach (var user in users)
    //     // {
    //     //     var result = await userManager.CreateAsync(user, "password");

    //     //     if (result.Succeeded)
    //     //     {
    //     //         r = roles[random.Next(0, 2)];
    //     //         await userManager.AddToRoleAsync(user, r);

    //     //     }
    //     //     else
    //     //     {
    //     //         throw new Exception("Failed to create user");
    //     //     }


    //     // }
    //     Console.WriteLine("Seeding done");

    //     context.Users.AddRange(users);
    //     context.SaveChanges();
    // }
}
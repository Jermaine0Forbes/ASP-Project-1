using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using WebApplication1.Data;
using System;
using System.Linq;
using Bogus;
using NuGet.Protocol;
using System.Text.Json;

namespace WebApplication1.Models;

public static class SeedData
{

    public static async Task Initialize(IServiceProvider serviceProvider)
    {

        using var context = new AppDBContext(
    serviceProvider.GetRequiredService<
        DbContextOptions<AppDBContext>>());

        using var transaction = context.Database.BeginTransaction();

            // 1. Enable IDENTITY_INSERT for the 'Posts' table
            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Posts ON");

        try
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roles = ["User", "Manager", "Admin"];



            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }


            // Look for any Users.
            if (context.Users.Any())
            {
                return;   // DB has been seeded
            }

            var fakeUsers = new Faker<User>()
            .RuleFor(u => u.UserName, f => f.Internet.UserName())
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.PasswordHash, f => f.Internet.Password())
            .RuleFor(u => u.CreatedAt, f => f.Date.Soon());
            // .FinishWith((f, u) =>
            // {
            //     context.Posts.Add(new Post
            //     {
            //         Title = u.UserName + ": Title",
            //         Body = f.Lorem.Sentence(),
            //         CreatedAt = u.CreatedAt,
            //         User = u,
            //     });

            // });

            var fakePosts = new Faker<Post>()
            .RuleFor(p => p.Title, f => f.Lorem.Sentence(3))
            .RuleFor(p => p.Body, f => f.Lorem.Paragraph());


            var users = fakeUsers.Generate(20);
            List<Post> posts = fakePosts.Generate(100);

            // context.Users.AddRange(users);
            // context.SaveChanges();

            //    Console.WriteLine(JsonSerializer.Serialize(posts));

            //    return;


            var r = SeedData.GetRole();
            foreach (var user in users)
            {
                var u = new User
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    PasswordHash = "Password123!",
                    CreatedAt = user.CreatedAt,
                };
                var result = await userManager.CreateAsync(u, "Password123!");
                if (result.Succeeded)
                {
                    r = SeedData.GetRole();
                    await userManager.AddToRoleAsync(u, r);
                    context.Posts.Add(SeedData.GetPost(u, posts));
                    // await context.SaveChangesAsync();

                }
                else
                {

                    throw new Exception(result.Errors.ToJson());
                }


            }

            await context.SaveChangesAsync();


            // 3. Disable IDENTITY_INSERT
            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Posts OFF");


            transaction.Commit();

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }


    }

    public static string GetRole()
    {
        Random random = new();
        var num = random.Next(1, 1000);

        var value = num switch
        {
            int n when n > 666 => "Admin",
            int n when n > 333 => "Manager",
            _ => "User"
        };

        return value;


    }

    public static Post GetPost(User u, List<Post> p)
    {
        Random random = new();
        var num = random.Next(0, 99);
        var post = p[num];

        return new Post
        {
            Title = post.Title + ":" + u.UserName + ": Title",
            Body = post.Body,
            CreatedAt = u.CreatedAt,
            // UserId = u.Id,
            User = u,
        };
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
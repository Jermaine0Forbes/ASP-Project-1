using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using WebApplication1.Data;
using WebApplication1.Claims;
using System;
using System.Linq;
using Bogus;
using NuGet.Protocol;

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
        // context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Posts ON");

        try
        {

            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
            string[] roles = ["User", "Manager", "Admin"];


            foreach (var role in roles)
            {

                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new Role(role));
                    await RoleClaims.AddClaims(role, roleManager);
                }
            }


            // Look for any Users.
            if (context.Users.Any())
            {
                return;   // DB has been seeded
            }

            DateTime currentDateTime = DateTime.Now;

            var fakeUsers = new Faker<User>()
            .RuleFor(u => u.UserName, f => f.Internet.UserName())
            .RuleFor(u => u.Email, (f, u) => u.UserName + "@gmail.com")
            .RuleFor(u => u.CreatedAt, f => currentDateTime)
            .RuleFor(u => u.EmailConfirmed, f => true)
            .RuleFor(u => u.OtpExpirationDate, f => DateTime.Now.AddDays(5));
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


            foreach (var user in users)
            {

                var result = await userManager.CreateAsync(user, "Password123!");
                if (result.Succeeded)
                {
                    var r = SeedData.GetRole();
                    if (!await userManager.IsInRoleAsync(user, r))
                    {
                        await userManager.AddToRoleAsync(user, r);

                    }
                    await context.Posts.AddAsync(SeedData.GetPost(user, posts));

                }
                else
                {

                    throw new Exception(result.Errors.ToJson());
                }


            }

            await context.SaveChangesAsync();

            // 3. Disable IDENTITY_INSERT
            // context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Posts OFF");

            transaction.Commit();

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            await transaction.RollbackAsync();
            throw new Exception(e.Message);
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
            Title = u.UserName + " Post : " + post.Title,
            Body = post.Body,
            CreatedAt = u.CreatedAt,
            UserId = u.Id,
            // User = u,
        };
    }

}
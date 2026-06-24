using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using WebApp2.Models;
using System;
using System.Linq;
using Bogus;
using NuGet.Protocol;

namespace WebApp2.Data;
public static class Seeder
{
    


    public static async Task Initialize(IServiceProvider serviceProvider)
    {

        using var context = new AppDBContext(
    serviceProvider.GetRequiredService<
        DbContextOptions<AppDBContext>>());

        using var transaction = context.Database.BeginTransaction();


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

            DateTime currentDateTime = DateTime.Now;

            var fakeUsers = new Faker<User>()
            .RuleFor(u => u.UserName, f => f.Internet.UserName())
            .RuleFor(u => u.Email, (f, u) => u.UserName + "@gmail.com")
            .RuleFor(u => u.CreatedAt, f => currentDateTime)
            .RuleFor(u => u.EmailConfirmed, f => true);


            var fakePosts = new Faker<Post>()
            .RuleFor(p => p.Title, f => f.Lorem.Sentence(3))
            .RuleFor(p => p.Body, f => f.Lorem.Paragraph());


            var users = fakeUsers.Generate(20);
            List<Post> posts = fakePosts.Generate(100);


            foreach (var user in users)
            {

                var result = await userManager.CreateAsync(user, "Password123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "User");
                    await context.Posts.AddAsync(Seeder.GetPost(user, posts));

                }
                else
                {

                    throw new Exception(result.Errors.ToJson());
                }


            }

            await context.SaveChangesAsync();

            transaction.Commit();

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            await transaction.RollbackAsync();
            throw new Exception(e.Message);
        }

    }

    public static Post GetPost(User u, List<Post> p)
    {
        Random random = new();
        var num = random.Next(0, 99);
        var post = p[num];

        return new Post
        {
            Title = u.UserName + " Post - " + post.Title,
            Body = post.Body,
            CreatedAt = u.CreatedAt,
            UserId = u.Id,
        };
    }

}
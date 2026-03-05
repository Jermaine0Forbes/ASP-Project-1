using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using WebApplication1.Data;
using System;
using System.Linq;
using Bogus;

namespace WebApplication1.Models;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new AppDBContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<AppDBContext>>()))
        {
            using var scope = serviceProvider.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService(RoleManager<IdentityRole>)();
            // Look for any Userss.
            if (context.Users.Any())
            {
                return;   // DB has been seeded
            }
            var fakeUsers = new Faker<User>()
            // .RuleFor(u => u.Id , f => userId++)
            .RuleFor(u => u.Username , f => f.Internet.UserName())
            .RuleFor(u => u.Email , f => f.Internet.Email())
            .RuleFor(u => u.PasswordHash , f => f.Internet.Password())
            .RuleFor(u => u.CreatedAt , f => f.Date.Soon())
            .RuleFor(u => u.UpdatedAt , f => f.Date.Soon());

            var users = fakeUsers.Generate(10);

            context.Users.AddRange(users);
            context.SaveChanges();
        }
    }
}
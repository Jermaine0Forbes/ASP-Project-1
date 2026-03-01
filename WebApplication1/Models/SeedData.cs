using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
            // Look for any UserModels.
            if (context.UserModel.Any())
            {
                return;   // DB has been seeded
            }
            var userId = 1;
            var fakeUsers = new Faker<UserModel>()
            // .RuleFor(u => u.Id , f => userId++)
            .RuleFor(u => u.Username , f => f.Internet.UserName())
            .RuleFor(u => u.Email , f => f.Internet.Email())
            .RuleFor(u => u.Password , f => f.Internet.Password())
            .RuleFor(u => u.CreatedAt , f => f.Date.Soon())
            .RuleFor(u => u.UpdatedAt , f => f.Date.Soon());

            var users = fakeUsers.Generate(10);

            context.UserModel.AddRange(users);
            context.SaveChanges();
        }
    }
}
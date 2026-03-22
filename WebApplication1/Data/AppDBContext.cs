using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class AppDBContext : IdentityDbContext<User>
    {
        public AppDBContext(DbContextOptions<AppDBContext> options)
            : base(options)
        {
        }

        protected override void  OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // builder.Entity<User>()
            // .HasMany(u => u.Posts)
            // .WithOne(p => p.User)
            // .HasForeignKey(p => p.User != null ? p.User.Id : null);

        }


        // public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
    }
}

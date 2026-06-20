using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApp2.Models;

namespace WebApp2.Data;

public class AppDBContext(DbContextOptions<AppDBContext> options) : IdentityDbContext(options)
{
    
    public DbSet<Post> Posts { get; set; }
}

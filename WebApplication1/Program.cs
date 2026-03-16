using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WebApplication1.Data;
using WebApplication1.Models;
using System.Threading.RateLimiting;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("AppDBContext") ?? throw new InvalidOperationException("Connection string 'AppDBContext' not found.");
builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseSqlServer());

builder.Services.AddDbContext<AppDBContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<AppDBContext>();

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("LoginPolicy", config =>
    {
        config.PermitLimit = 5; // Allow a maximum of 4 requests
        config.Window = TimeSpan.FromMinutes(5); // per 1 minute window
        config.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        config.QueueLimit = 0; // Set to 0 to reject extra requests immediately
        config.AutoReplenishment = true;
    });
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests; // Return HTTP 429

    // Partition by IP address before the user is authenticated
    options.OnRejected = (context, cancellationToken) =>
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        context.HttpContext.Response.WriteAsync("Too many login attempts. Please try again later.", cancellationToken);
        return ValueTask.CompletedTask;
    };
});


// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;

    //    SeedData.Initialize(services);
       await  SeedData.Initialize(services);
    }
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets().RequireRateLimiting("LoginPolicy");


app.Run();

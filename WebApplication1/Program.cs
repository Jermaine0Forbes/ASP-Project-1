using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Google;
using WebApplication1.Configurations;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.DependencyInjection.Extensions;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Services;
using WebApplication1.Middlewares;
using System.Threading.RateLimiting;
using Serilog;
using Serilog.AspNetCore;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("AppDBContext") ?? throw new InvalidOperationException("Connection string 'AppDBContext' not found.");
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("GmailOptions") ?? throw new InvalidOperationException("GmailOptions not found."));
builder.Services.AddDbContext<AppDBContext>(options =>
{
    options.UseSqlServer(connectionString);
    //   .LogTo(Console.WriteLine, LogLevel.Information);

});
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console(
        outputTemplate:
        "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"
    )
    .WriteTo.File(
        path: "Logs/app-.log",
        rollingInterval: RollingInterval.Day,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
        retainedFileCountLimit: 30)
    .WriteTo.File(
        path: "Logs/sql-.log",
        rollingInterval: RollingInterval.Day,
        restrictedToMinimumLevel: LogEventLevel.Information,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [SQL] {Message:lj}{NewLine}{Exception}",
        retainedFileCountLimit: 30,
        shared: false)
    .CreateLogger();

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext() // Important for DiagnosticContext
                                 // .WriteTo.Console()
         .WriteTo.File("Logs/request-.log", rollingInterval: RollingInterval.Day)
);

builder.Services.AddDefaultIdentity<User>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;

    options.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultAuthenticatorProvider;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<AppDBContext>()
.AddDefaultTokenProviders();

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



builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
}).AddGoogle(options =>
{
    options.ClientId =
    builder.Configuration.GetValue<string>("Authentication:Google:ClientId")
    ?? throw new InvalidOperationException("Google Client Id not found.");
    options.ClientSecret =
    builder.Configuration.GetValue<string>("Authentication:Google:ClientSecret")
    ?? throw new InvalidOperationException("Google Client Secret not found.");
    options.CallbackPath = "/signin-google";

    // Optional but useful
    options.SaveTokens = true;

    options.Scope.Add("profile");
    options.Scope.Add("email");
});



builder.Services.AddScoped<EmailService>();
builder.Services.AddSingleton<IAuthorizationHandler, UserOwnerHandler>();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("IsOwnerOrAuthorized", policy =>
        policy.Requirements.Add(new OwnerAuthorizationRequirement(["Manager, Admin"])));
    options.AddPolicy("IsOwner", policy =>
        policy.Requirements.Add(new OwnerAuthorizationRequirement()));
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
        await SeedData.Initialize(services);
    }
}
app.UseMiddleware<ErrorLoggingMiddleware>();

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseRateLimiter();
app.UseSerilogRequestLogging();


app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();
// .WithStaticAssets().RequireRateLimiting("LoginPolicy");

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.Run();

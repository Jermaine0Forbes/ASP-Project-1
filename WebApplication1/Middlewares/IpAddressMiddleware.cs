using IP2LocationIOComponent;
using System.Net.Http;
using System.Security.Claims;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Middlewares
{
    public class IpAddressMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string[] _ignoreExtensions = [".js", ".ts", ".json", ".css", ".scss", ".png", ".jpg", ".map", ".ico", "xml"];
        private readonly string[] _ignoreIps = ["::1", "127.0.0.1"];


        private readonly ILogger<IpAddressMiddleware> _logger;



        public IpAddressMiddleware(RequestDelegate next, ILogger<IpAddressMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        private bool isEndPoint(HttpContext context)
        {
            return !_ignoreExtensions.Any(ext =>
            context.Request.Path.Value != null ?
            context.Request.Path.Value.EndsWith(ext, StringComparison.OrdinalIgnoreCase)
            : false
            );
        }

        private async Task<string> GetIp(HttpContext context)
        {
            // Access the client's IP address
            var remoteIp = context.Connection.RemoteIpAddress?.ToString();
            var ignoreIp = _ignoreIps.Any(ip => ip == remoteIp);
            if (ignoreIp)
            {
                using var client = new HttpClient();
                string publicIp = await client.GetStringAsync("https://api.ipify.org");

                return publicIp;
            }

            return remoteIp ?? "";
        }

        public async Task InvokeAsync(HttpContext context, AppDBContext dbcontext)
        {

            if (isEndPoint(context))
            {
                // Configures IP2Location.io API key
                Configuration Config = new()
                {
                    ApiKey = "E9A1ADCCF63B2DFF08FF5854D511FABE"
                };
                IPGeolocation IPL = new(Config);
                var remoteIp = await GetIp(context);
                var endpoint = await IPL.Lookup(remoteIp);

                // Add the IP to response headers
                context.Response.Headers.Append("X-Client-IP", remoteIp);
                var authenticated = context?.User?.Identity?.IsAuthenticated ?? false;
                Console.WriteLine(context?.User?.Identity?.Name);
                var userId = authenticated ? context?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value : "";
                var path = context?.Request.Path ?? null;

                IpAddress ia = new()
                {
                    Path = path,
                    Address = remoteIp,
                    UserId = userId ?? null,
                    Latitude = endpoint["latitude"]?.ToString() ?? "",
                    Longitude = endpoint["longitude"]?.ToString() ?? "",
                    CountryCode = endpoint["country_code"]?.ToString() ?? "",
                    CountryName = endpoint["country_name"]?.ToString() ?? "",
                    Region = endpoint["region_name"]?.ToString() ?? "",
                    City = endpoint["city_name"]?.ToString() ?? "",
                    Zip = endpoint["zip_code"]?.ToString() ?? "",
                    CreatedAt = DateTime.Now

                };

                await dbcontext.IpAddresses.AddAsync(ia);
                await dbcontext.SaveChangesAsync();


                var user = authenticated ? userId : "unauthenticated";

                _logger.LogWarning("User: {0}, Address: {1}, Path: {2}", user, remoteIp, path);
                _logger.LogWarning("User: {0}, Address: {1}, Zip code: {2}", user, remoteIp, endpoint["zip_code"]?.ToString() ?? "cannot find zip code");

            }

            await _next(context!);
        }
    }


}

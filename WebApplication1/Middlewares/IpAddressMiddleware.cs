using IP2LocationIOComponent;
using System.Net.Http;

namespace WebApplication1.Middlewares
{
    public class IpAddressMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string[] _ignoreExtensions = [".js", ".ts", ".json", ".css", ".scss", ".png", ".jpg", ".map", ".ico"];
        private readonly string[] _ignoreIps = ["::1", "127.0.0.1"];

        public IpAddressMiddleware(RequestDelegate next)
        {
            _next = next;
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

        public async Task InvokeAsync(HttpContext context)
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

                // Example: Add the IP to response headers
                context.Response.Headers.Append("X-Client-IP", remoteIp);
                Console.WriteLine("Path is {0}", context.Request.Path);
                Console.WriteLine("Ip address is {0}", remoteIp);
                Console.WriteLine("endpoint {0}", endpoint["country_name"]);

            }

            await _next(context);
        }
    }


}

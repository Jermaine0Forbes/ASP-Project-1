public class IpAddressMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string[] _ignoreExtensions = [".js", ".ts", ".json", ".css", ".scss", ".png", ".jpg", ".map", ".ico"];

    public IpAddressMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    private Boolean isEndPoint(HttpContext context)
    {
        return !_ignoreExtensions.Any(ext => 
        context.Request.Path.Value!=null ? 
        context.Request.Path.Value.EndsWith(ext, StringComparison.OrdinalIgnoreCase) 
        : false
        );
    }

    public async Task InvokeAsync(HttpContext context)
    {

        if(isEndPoint(context))
        {
            // Access the client's IP address
            var remoteIp = context.Connection.RemoteIpAddress?.ToString();

            // Example: Add the IP to response headers
            context.Response.Headers.Append("X-Client-IP", remoteIp);
            Console.WriteLine("Path is {0}", context.Request.Path);
            Console.WriteLine("Ip address is {0}", remoteIp);
            
        }

        await _next(context);
    }
}

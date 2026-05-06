public class IpAddressMiddleware
{
    private readonly RequestDelegate _next;

    public IpAddressMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Access the client's IP address
        var remoteIp = context.Connection.RemoteIpAddress?.ToString();

        // Example: Add the IP to response headers
        context.Response.Headers.Append("X-Client-IP", remoteIp);

        await _next(context);
    }
}

namespace Gamestore.API.Middlewares;

public class RequestLoggingMiddleware(
    ILogger<RequestLoggingMiddleware> logger,
    RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var startTime = DateTime.UtcNow;

        await next.Invoke(context);

        var duration = DateTime.UtcNow - startTime;

        var clientIpAddress = context.Connection.RemoteIpAddress.ToString();

        logger.LogInformation(
            "Request from IP: {ip} {method} {url} took {duration}ms",
            clientIpAddress,
            context.Request.Method,
            context.Request.Path,
            duration.TotalMilliseconds);
    }
}
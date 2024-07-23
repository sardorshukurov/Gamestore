using System.Text;

namespace Gamestore.API.Middlewares;

public class RequestLoggingMiddleware(ILogger<RequestLoggingMiddleware> logger) : IMiddleware
{
    private readonly ILogger<RequestLoggingMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var request = context.Request;

        _logger.LogInformation($"Processing request from IP: {request.HttpContext.Connection.RemoteIpAddress} to URL: {request.Path}");
        _logger.LogInformation($"Request content: {ReadRequestBody(request)}");

        var buffer = new MemoryStream();
        var stream = context.Response.Body;
        context.Response.Body = buffer;

        await next(context);

        buffer.Position = 0;
        var responseText = await new StreamReader(buffer).ReadToEndAsync();
        _logger.LogInformation($"Response content: {responseText}");

        buffer.Position = 0;
        await buffer.CopyToAsync(stream);
        context.Response.Body = stream;
    }

    private static string ReadRequestBody(HttpRequest request)
    {
        request.EnableBuffering();

        var buffer = new byte[Convert.ToInt32(request.ContentLength)];
        var bodyAsText = Encoding.UTF8.GetString(buffer);
        request.Body.Position = 0;

        return bodyAsText;
    }
}
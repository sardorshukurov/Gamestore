using System.Diagnostics;
using System.Text;

namespace Gamestore.API.Middlewares;

public class RequestLoggingMiddleware(ILogger<RequestLoggingMiddleware> logger) : IMiddleware
{
    private readonly ILogger<RequestLoggingMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var request = context.Request;

        // TODO: most of this logs Serilog has out of the box to Log API requests and responses
        _logger.LogInformation($"Processing request from IP: {request.HttpContext.Connection.RemoteIpAddress} to URL: {request.Path}");
        _logger.LogInformation($"Request content: {ReadRequestBody(request)}");

        var buffer = new MemoryStream();
        var stream = context.Response.Body;
        context.Response.Body = buffer;

        var stopWatch = Stopwatch.StartNew();
        await next(context);
        stopWatch.Stop();

        _logger.LogInformation($"Response status code: {context.Response.StatusCode}");

        buffer.Position = 0;
        var responseText = await new StreamReader(buffer).ReadToEndAsync();
        _logger.LogInformation($"Response content: {responseText}");
        _logger.LogInformation($"Elapsed time: {stopWatch.Elapsed}");

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
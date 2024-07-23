using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace Gamestore.API.Middlewares;

public class GlobalExceptionHandlingMiddleware(ILogger<GlobalExceptionHandlingMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            LogException(ex);

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            ProblemDetails problem = new()
            {
                Status = (int)HttpStatusCode.InternalServerError,
                Type = "Server error",
                Title = "Server error",
                Detail = "An internal server error has occured",
            };

            string json = JsonSerializer.Serialize(problem);

            await context.Response.WriteAsync(json);

            context.Response.ContentType = "application/json";
        }
    }

    private void LogException(Exception ex)
    {
        var exception = ex;

        while (exception != null)
        {
            logger.LogError("{Type}: {Message} {StackTrace}", exception.GetType().Name, exception.Message, exception.StackTrace);

            exception = exception.InnerException;
        }
    }
}
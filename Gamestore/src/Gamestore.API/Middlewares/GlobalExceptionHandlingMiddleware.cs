using System.Net;
using Gamestore.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Gamestore.API.Middlewares;

public class GlobalExceptionHandlingMiddleware(
    ILogger<GlobalExceptionHandlingMiddleware> logger,
    RequestDelegate next,
    IHostEnvironment env)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            LogException(exception);
            await HandleExceptionAsync(context, exception);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = GetHttpStatusCode(exception);

        var detailMessage = exception.InnerException != null ? exception.InnerException.Message : exception.Message;
        var problemDetails = new ProblemDetails
        {
            Type = exception.GetType().Name,
            Title = statusCode.ToString(),
            Status = (int)statusCode,
            Detail = detailMessage,
            Instance = $"urn:{context.TraceIdentifier}",
        };

        var result = JsonConvert.SerializeObject(problemDetails);

        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = (int)statusCode;

        if (env.IsDevelopment())
        {
            await context.Response.WriteAsync(result);
        }
    }

    private static HttpStatusCode GetHttpStatusCode(Exception ex)
    {
        return ex switch
        {
            NotFoundException => HttpStatusCode.NotFound,
            BadRequestException => HttpStatusCode.BadRequest,
            _ => HttpStatusCode.InternalServerError,
        };
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
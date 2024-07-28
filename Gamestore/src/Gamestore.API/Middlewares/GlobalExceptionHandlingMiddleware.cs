using System.Net;
using System.Text.Json;
using Gamestore.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Gamestore.API.Middlewares;

public class GlobalExceptionHandlingMiddleware(
    ILogger<GlobalExceptionHandlingMiddleware> logger,
    RequestDelegate next,
    IHostEnvironment env)
{
    public async Task InvokeAsync(HttpContext context, CancellationToken cancellationToken)
    {
        try
        {
            await next(context);
        }
        catch (NotFoundException nfex)
        {
            LogException(nfex);

            context.Response.StatusCode = (int)HttpStatusCode.NotFound;

            string json = JsonSerializer.Serialize(nfex.Message);

            await context.Response.WriteAsync(json, cancellationToken);

            context.Response.ContentType = "application/json";
        }
        catch (BadRequestException brex)
        {
            LogException(brex);

            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            string json = JsonSerializer.Serialize(brex.Message);

            await context.Response.WriteAsync(json, cancellationToken);

            context.Response.ContentType = "application/json";
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
                Detail = env.IsDevelopment() ? ex.Message + ex.StackTrace : "An internal server error has occured",
            };

            context.Response.ContentType = "application/json";

            string json = JsonSerializer.Serialize(problem);

            await context.Response.WriteAsync(json, cancellationToken);
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
using System.Net;
using System.Text.Json;
using TextAnalyzer.Application.Exceptions;

namespace TextAnalyzer.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (InvalidTextException ex)
        {
            _logger.LogWarning("Invalid text input: {Message}", ex.Message);
            await WriteErrorResponse(context, HttpStatusCode.BadRequest, ex.Message);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "External service call failed");
            await WriteErrorResponse(context, HttpStatusCode.ServiceUnavailable, 
                "An external service is unavailable. Please try again later.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error occurred");
            await WriteErrorResponse(context, HttpStatusCode.InternalServerError, 
                "An unexpected error occurred.");
        }
    }
    
    private static async Task WriteErrorResponse(
        HttpContext context, 
        HttpStatusCode statusCode, 
        string message)
    {
        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";

        var response = new { error = message, statusCode = (int)statusCode };
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
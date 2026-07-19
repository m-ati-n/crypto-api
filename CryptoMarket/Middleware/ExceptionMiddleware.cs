using System.Text.Json;
using CryptoMarket.DTOs;

namespace CryptoMarket.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger)
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
        catch (Exception ex)
        {
            context.Response.StatusCode = ex switch
            {
                ArgumentException => StatusCodes.Status400BadRequest,
                KeyNotFoundException => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError
            };
            context.Response.ContentType = "application/json";
            
            _logger.LogError(ex,
            "Unhandled exception occurred.");
            
            var response = new ErrorResponseDto
            {
                StatusCode = context.Response.StatusCode,
                Message = ex.Message,
                TimeStamp = DateTime.UtcNow,
                Path = context.Request.Path
            };

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response));
        }
    }
}
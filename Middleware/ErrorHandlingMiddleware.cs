using System.Net;
using EasyDine.DTOs;
using Microsoft.EntityFrameworkCore;

namespace EasyDine.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;
    
    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database update error occurred.");
            
            context.Response.StatusCode = (int)HttpStatusCode.Conflict;
            context.Response.ContentType = "application/json";
            
            var message = ex.InnerException?.Message.Contains("UNIQUE") == true
                ? "A unique constraint violation occurred."
                : "A database error occurred.";
            
            var response = ApiResponse<string>.Fail(message);
            await context.Response.WriteAsJsonAsync(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred.");
            
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            
            var response = ApiResponse<string>.Fail("An unexpected error occurred. Please try again later.");
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
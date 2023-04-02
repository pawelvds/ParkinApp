using System.Net;
using ParkinApp.Domain.Common.Errors;

namespace ParkinApp.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger; 

    private const string ExceptionParseErrorMessage = "Something went wrong while parsing the exception.";

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger) 
    {
        _logger = logger;
        _next = next;
    }
    
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            _logger.LogError("Something went wrong: {exception}", ex);
            await HandleExceptionAsync(httpContext, ex);
        }
    }
    
    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        
        await context.Response.WriteAsync(new Error
        {
            StatusCode = context.Response.StatusCode,
            Message = exception.Message,
            ExceptionType = typeof(Exception).ToString()
        }.ToString() ?? throw new InvalidOperationException(ExceptionParseErrorMessage));
    }
}
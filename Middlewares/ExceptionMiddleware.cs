using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var error = true;
        var errorMessage = "";
        try
        {
            await _next(context);
            error = false;
        }
        catch (InvalidOperationException ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            errorMessage = ex.Message;
        }
        catch (ArgumentException ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            errorMessage = ex.Message;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Something went wrong: {ex}");
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            errorMessage = "An unexpected error occurred.";
        }
        finally {
            if (error) {
                await HandleExceptionAsync(context, errorMessage);
            }
        }
    }

    private Task HandleExceptionAsync(HttpContext context, String msg)
    {
        context.Response.ContentType = "application/json";

        return context.Response.WriteAsync(new ErrorDetails
        {
            StatusCode = context.Response.StatusCode,
            Message = msg
        }.ToString());
    }
}

public class ErrorDetails
{
    public int StatusCode { get; set; }
    public string Message { get; set; }

    public override string ToString()
    {
        return System.Text.Json.JsonSerializer.Serialize(this);
    }
}
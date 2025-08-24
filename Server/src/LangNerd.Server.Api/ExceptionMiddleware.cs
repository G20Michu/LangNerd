using Microsoft.AspNetCore.Components.Forms.Mapping;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using LangNerd.Server.Api.Exceptions;

namespace LangNerd.Server.Api.Middleware;
internal sealed class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context,IExceptionMapperRoot mapper)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred");
            var mapped = mapper.Map(ex);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = mapped.StatusCode;

            context.Response.StatusCode = ex switch
            {
                AppException => 400,           //App Error
                _ => 500                       //Internal Server Error
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                message = mapped.Message,
                code = mapped.StatusCode
            }));
        }
    }
}

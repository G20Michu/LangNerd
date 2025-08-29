using System.Text.Json;
using LangNerd.Server.Api.Exceptions;

namespace LangNerd.Server.Api.Middleware;
internal sealed class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context,IExceptionMapperRoot mapper)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            var mapped = mapper.Map(ex);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = mapped.StatusCode;

            await context.Response.WriteAsJsonAsync(JsonSerializer.Serialize(new
            {
                message = mapped.Message,
                code = mapped.Code,
                statusCode = mapped.StatusCode,
            }));
        }
    }
}

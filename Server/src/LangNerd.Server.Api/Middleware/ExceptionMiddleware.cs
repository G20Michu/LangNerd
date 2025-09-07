using System.Text.Json;
using LangNerd.Server.Api.Exceptions;

namespace LangNerd.Server.Api.Middleware;
internal sealed class ExceptionMiddleware(IExceptionMapperRoot mapper) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            var mapped = mapper.Map(ex);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = mapped.StatusCode;

            await context.Response.WriteAsJsonAsync(new
            {
                message = mapped.Message,
                code = mapped.Code,
            });
        }
    }
}

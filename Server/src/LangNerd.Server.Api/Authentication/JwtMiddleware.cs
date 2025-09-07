using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace LangNerd.Server.Api.Authentication;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public class JwtMiddleware : Attribute
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;

    public JwtMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var allowAnonymousAttribute = context
                 .GetEndpoint()
                ?.Metadata
                ?.GetMetadata<AllowAnonymousAttribute>();
        var endpoint = context
                        .GetEndpoint();
        if (endpoint == null)
        {
            Console.WriteLine("not mapped");
            await _next(context);
            return;
        }

        if (allowAnonymousAttribute != null)
        {
            Console.WriteLine("anonymous");

            await _next(context);
            return;
        }

        var token = context.Request.Cookies["jwt"];
        if (!string.IsNullOrEmpty(token))
        {
            Console.WriteLine("auth");

            var username = JwtService.ValidateJwt(token, _configuration);
            if (username == null)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized");
            }
            else
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, username), 
                };
                var identity = new ClaimsIdentity(claims, "TestAuthType");
                var principal = new ClaimsPrincipal(identity);
                context.User = principal;

                await _next(context);
            }
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized");
        }
    }
}
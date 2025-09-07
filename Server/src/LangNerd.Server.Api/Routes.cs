using LangNerd.Server.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
namespace LangNerd.Server.Api;

public static class Routes
{
    public static void MapAppRoutes(this IEndpointRouteBuilder app)
    {
        app.MapGet("/", (HttpContext httpContext) => RoutesFunc.MainPage(httpContext)).RequireAuthorization();
        app.MapPost("/login", (LoginDto dto, SignInManager<IdentityUser> signInManager, IConfiguration configuration, HttpContext http)
        => RoutesFunc.Login(dto, signInManager, configuration, http)).WithMetadata(new AllowAnonymousAttribute());

        app.MapPost("/register", (RegisterDto dto, UserManager<IdentityUser> userManager) => RoutesFunc.Register(dto, userManager)).WithMetadata(new AllowAnonymousAttribute());
        app.MapGet("/JwtTest", (HttpContext httpContext) => RoutesFunc.MainPage(httpContext)).WithMetadata(new AllowAnonymousAttribute());
    }

}
public class RoutesFunc
{
    public static string MainPage(HttpContext httpContext)
    {
        return $"Hello {httpContext.User.Identity!.Name}";
    }

    public static async Task<IResult> Login(LoginDto dto, SignInManager<IdentityUser> signInManager, IConfiguration configuration, HttpContext http)
    {
        var result = await signInManager.PasswordSignInAsync(dto.Username, dto.Password, false, false);

        if (result.Succeeded)
        {
            return Results.Ok("User logged in successfully");
        }
        else
        {
            return Results.BadRequest("Login failed");
        }
    }

    public static async Task<string> Register(RegisterDto dto, UserManager<IdentityUser> userManager)
    {
        var user = new IdentityUser { UserName = dto.Username, Email = dto.Email };
        var result = await userManager.CreateAsync(user, dto.Password);

        if (result.Succeeded)
            return "User created successfully";

        return string.Join("; ", result.Errors.Select(e => e.Description));
    }
}
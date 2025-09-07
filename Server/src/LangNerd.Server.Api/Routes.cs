using LangNerd.Server.Api.Exceptions;
using LangNerd.Server.Api.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Authentication;
namespace LangNerd.Server.Api;

public static class Routes
{
    public static void MapAppRoutes(this IEndpointRouteBuilder app)
    {
        app.MapGet("/", (HttpContext httpContext) => RoutesFunc.MainPage(httpContext)).RequireAuthorization();
        app.MapPost("/login", (LoginDto dto, SignInManager<IdentityUser> signInManager)
        => RoutesFunc.Login(dto, signInManager));
        app.MapPost("/register", (RegisterDto dto, UserManager<IdentityUser> userManager) => RoutesFunc.Register(dto, userManager));
    }

}
public class RoutesFunc
{
    public static string MainPage(HttpContext httpContext)
    {
        return $"Hello {httpContext.User.Identity!.Name}";
    }

    public static async Task Login(LoginDto dto, SignInManager<IdentityUser> signInManager)
    {
        var result = await signInManager.PasswordSignInAsync(dto.Username, dto.Password, false, false);

        if (!result.Succeeded)
        {
            throw new InvalidCredentialException();
        }
    }

    public static async Task Register(RegisterDto dto, UserManager<IdentityUser> userManager)
    {
        var user = new IdentityUser { UserName = dto.Username, Email = dto.Email };
        var result = await userManager.CreateAsync(user, dto.Password);

        if(!result.Succeeded)
        {
            throw new RegisterFailedException();
        }
    }
}
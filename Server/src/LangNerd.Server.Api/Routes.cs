using System;
using System.Threading.Tasks;
using LangNerd.Server.Api.Models;
using Microsoft.AspNetCore.Identity;

namespace LangNerd.Server.Api.Exceptions;
public static class Routes
{
    public static void MapAppRoutes(this IEndpointRouteBuilder app)
    {
    app.MapGet("/", () => RoutesFunc.MainPage());
    app.MapPost("/login", (LoginDto dto, SignInManager<IdentityUser> signInManager) => RoutesFunc.Login(dto, signInManager));
    app.MapPost("/register", (RegisterDto dto, UserManager<IdentityUser> userManager) => RoutesFunc.Register(dto, userManager));
    }

}
public class RoutesFunc
{
    public static string MainPage()
    {
        return $"Main Page {DateTime.Now}";
    }

    public static async Task<string> Login(LoginDto dto, SignInManager<IdentityUser> signInManager)
    {
        var result = await signInManager.PasswordSignInAsync(dto.Username, dto.Password, false, false);
        return result.Succeeded ? "User logged in successfully" : "Login failed";
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
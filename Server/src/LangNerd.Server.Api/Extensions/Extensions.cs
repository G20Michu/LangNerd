using LangNerd.Server.Api.Database;
using LangNerd.Server.Api.Exceptions;
using LangNerd.Server.Api.Middleware;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LangNerd.Server.Api.Extensions;

public static class Extensions
{
    public static void AddApplication(this WebApplicationBuilder builder)
    {
        LoadEnv();
        builder.Configuration.AddEnvironmentVariables();
        builder.Services.ConfigureDbContext(builder.Configuration);
        builder.Services.AddSingleton<IExceptionMapperRoot, ExceptionMapperRoot>();
        builder.Services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
        builder.Services.AddControllersWithViews();
    }

    private static void LoadEnv()
    {
        var envFile = Path.Combine(Directory.GetCurrentDirectory(), ".env");

        if (!File.Exists(envFile))
            throw new FileNotFoundException($"Not Found: {envFile}");

        foreach (var line in File.ReadAllLines(envFile))
        {
            var trimmed = line.Trim();

            if (string.IsNullOrWhiteSpace(trimmed) || trimmed.StartsWith('#'))
                continue;

            var parts = trimmed.Split('=', 2);
            if (parts.Length == 2)
            {
                var key = parts[0].Trim();
                var value = parts[1].Trim();
                Console.WriteLine($"key {key} , value {value}");
                Environment.SetEnvironmentVariable(key, value);
            }
        }
    }

    private static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var host = configuration.GetValue<string>("DB_HOST");
        var port = configuration.GetValue<string>("DB_PORT");
        var db = configuration.GetValue<string>("DB_NAME");
        var user = configuration.GetValue<string>("DB_USER");
        var password = configuration.GetValue<string>("DB_PASSWORD");
        var connectionString = $"Host={host};Port={port};Database={db};Username={user};Password={password}";
        services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));
    }

    public static void UseApplication(this WebApplication app)
    {
        app.UseHttpsRedirection();
        app.MapAppRoutes();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseMiddleware<ExceptionMiddleware>();
        app.MapDefaultControllerRoute();
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;
using LangNerd.Server.Api.Databse;
using Npgsql;
using LangNerd.Server.Api.Exceptions;
using LangNerd.Server.Api.Middleware;



Env.Load();

var host = Environment.GetEnvironmentVariable("DB_HOST");
var port = Environment.GetEnvironmentVariable("DB_PORT");
var db = Environment.GetEnvironmentVariable("DB_NAME");
var user = Environment.GetEnvironmentVariable("DB_USER");
var password = Environment.GetEnvironmentVariable("DB_PASSWORD");

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IExceptionMapperRoot, ExceptionMapperRoot>();

var connectionString = $"Host={host};Port={port};Database={db};Username={user};Password={password}";

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));   

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseHttpsRedirection();
app.MapAppRoutes();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ExceptionMiddleware>();
app.MapDefaultControllerRoute();
app.Run();

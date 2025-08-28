
using LangNerd.Server.Api.Exceptions;
using LangNerd.Server.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IExceptionMapperRoot,ExceptionMapperRoot>();
var app = builder.Build();

app.UseHttpsRedirection();

app.MapAppRoutes();
app.UseMiddleware<ExceptionMiddleware>();

app.Run();


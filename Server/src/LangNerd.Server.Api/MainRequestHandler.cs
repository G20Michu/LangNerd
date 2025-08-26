var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseHttpsRedirection();

app.MapAppRoutes();

app.UseMiddleware<ExceptionMiddleware>();

app.Run();


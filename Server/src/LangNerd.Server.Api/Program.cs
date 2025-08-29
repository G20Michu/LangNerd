using LangNerd.Server.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.AddApplication();

var app = builder.Build();
app.UseApplication();

app.Run();
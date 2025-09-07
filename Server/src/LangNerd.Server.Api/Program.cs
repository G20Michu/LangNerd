using LangNerd.Server.Api.Extensions;
using LangNerd.Server.Api.Authentication;
var builder = WebApplication.CreateBuilder(args);
builder.AddApplication();

var app = builder.Build();
await app.UseApplication();
app.Run();
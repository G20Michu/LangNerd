using System;
namespace LangNerd.Server.Api.Exceptions;
public static class Routes
{
    public static void MapAppRoutes(this IEndpointRouteBuilder app)
    {
        app.MapGet("/", () => RoutesFunc.MainPage());
        app.MapGet("/InternalError", (int a) => RoutesFunc.InternalError(a));
        app.MapGet("/InternalSystemError", (int a) => RoutesFunc.InternalSystemError(a));
        app.MapGet("/ControlledException", (int a) => RoutesFunc.ControlledException(a));
    }

}
public class RoutesFunc
{
    public static string MainPage()
    {
        return $"Main Page {DateTime.Now}";
    }
    public static int InternalError(int a)
    {
        try
        {
            a /= 0;
            return a;
        }
        catch
        {
            throw new Exception("Internal_Error");
        }

    }
    public static int InternalSystemError(int a)
    {
        a /= 0;
        return a;
    }
    public static int ControlledException(int a)
    {
        try
        {
            a = 2/a;
            return a;
        }
        catch
        {
            throw new AppException("Invalid data input");
        }
    }
}
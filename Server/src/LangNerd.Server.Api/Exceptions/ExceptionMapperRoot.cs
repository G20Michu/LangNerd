
using System.Security.Authentication;

namespace LangNerd.Server.Api.Exceptions;

public sealed class ExceptionMapperRoot : IExceptionMapperRoot
{
    public ExceptionResponse Map(Exception exception)
    {
        return exception switch
        {
            AppException appEx => new ExceptionResponse(
                appEx.Message,
                GetExceptionCode(exception),
                StatusCodes.Status400BadRequest
            ),
            InvalidCredentialException invalidCredentialException => new(
                "Invalid credentials",
                GetExceptionCode(exception),
                StatusCodes.Status400BadRequest
            ),
            _ => new ExceptionResponse(
                "An unexpected error occurred",
                "Server_Error",
                StatusCodes.Status500InternalServerError
            )
        };
    }

    private static string GetExceptionCode(Exception ex)
        => ex.GetType()
            .Name
            .Replace("Exception", "");
}

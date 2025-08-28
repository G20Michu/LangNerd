
namespace LangNerd.Server.Api.Exceptions;

public sealed class ExceptionMapperRoot : IExceptionMapperRoot
{
    public ExceptionResponse Map(Exception exception)
    {
        return exception switch
        {
            AppException appEx => new ExceptionResponse(
                appEx.Message,
                $"{exception.GetType().Name}",
                StatusCodes.Status400BadRequest
            ),

            _ => new ExceptionResponse(
                "An unexpected error occurred",
                "UNKNOWN_ERROR",
                StatusCodes.Status500InternalServerError
            )
        };
    }
}

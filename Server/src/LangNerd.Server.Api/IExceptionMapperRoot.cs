namespace LangNerd.Server.Api.Exceptions;

public interface IExceptionMapperRoot
{
    ExceptionResponse Map(Exception exception);
}

public record ExceptionResponse(string Message, string Code, int StatusCode);

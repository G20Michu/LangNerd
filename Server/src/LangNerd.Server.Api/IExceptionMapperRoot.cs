namespace LangNerd.Server.Api.Exceptions;

public interface IExceptionMapperRoot
{
    ExceptionResponse Map(Exception exception);
}

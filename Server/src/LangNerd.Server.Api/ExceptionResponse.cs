namespace LangNerd.Server.Api.Exceptions;

public record ExceptionResponse(string Message, string Code, int StatusCode);

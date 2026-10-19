namespace Notifications.API.Exceptions;

// Se lanza cuando los datos de entrada son inválidos (HTTP 400).
public class ValidationException(string errorCode, string message) : Exception(message)
{
    public string ErrorCode { get; } = errorCode;
}
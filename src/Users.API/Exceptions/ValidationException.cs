namespace Users.API.Exceptions;

/// <summary>Se lanza cuando los datos de entrada son inválidos (HTTP 400).</summary>
public class ValidationException(string errorCode, string message) : Exception(message)
{
    public string ErrorCode { get; } = errorCode;
}

namespace Users.API.Exceptions;

/// <summary>Se lanza cuando las credenciales no son válidas (HTTP 401).</summary>
public class UnauthorizedException(string errorCode, string message) : Exception(message)
{
    public string ErrorCode { get; } = errorCode;
}

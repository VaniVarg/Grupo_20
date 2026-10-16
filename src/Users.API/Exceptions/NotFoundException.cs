namespace Users.API.Exceptions;

/// <summary>Se lanza cuando un recurso no existe (HTTP 404).</summary>
public class NotFoundException(string errorCode, string message) : Exception(message)
{
    public string ErrorCode { get; } = errorCode;
}

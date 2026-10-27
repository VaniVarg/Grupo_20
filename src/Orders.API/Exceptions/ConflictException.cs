namespace Orders.API.Exceptions;

/// <summary>
/// Se lanza cuando la operación choca con el estado actual del recurso, por ejemplo una transición de estado no permitida (HTTP 409).
/// </summary>
public class ConflictException(string errorCode, string message) : Exception(message)
{
    public string ErrorCode { get; } = errorCode;
}

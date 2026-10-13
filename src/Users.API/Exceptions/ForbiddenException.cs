namespace Users.API.Exceptions;

/// <summary>Se lanza cuando el usuario está bloqueado y no puede iniciar sesión (HTTP 403).</summary>
public class ForbiddenException(string errorCode, string message) : Exception(message)
{
    public string ErrorCode { get; } = errorCode;
}

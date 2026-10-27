using Users.API.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace Users.API.ExceptionHandlers;

/// <summary>
/// Convierte UnauthorizedException en HTTP 401 (USR-003, credenciales incorrectas).
/// Si la excepción no es de su tipo devuelve false y el framework prueba el siguiente handler.
/// </summary>
public class UnauthorizedExceptionHandler(ILogger<UnauthorizedExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not UnauthorizedException ex)
            return false;

        // Los errores de negocio se registran como Warning: no son fallos del sistema.
        logger.LogWarning("Credenciales incorrectas. errorCode={ErrorCode} path={Path}", ex.ErrorCode, httpContext.Request.Path.Value);

        await ErrorResponseWriter.WriteAsync(
            httpContext,
            StatusCodes.Status401Unauthorized,
            "https://tools.ietf.org/html/rfc7235#section-3.1",
            "Unauthorized",
            "Las credenciales no son válidas.",
            ex.ErrorCode,
            ex.Message,
            cancellationToken);

        return true;
    }
}

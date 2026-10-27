using Users.API.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace Users.API.ExceptionHandlers;

/// <summary>
/// Convierte ForbiddenException en HTTP 403 (USR-004 y USR-005, cuenta bloqueada).
/// Si la excepción no es de su tipo devuelve false y el framework prueba el siguiente handler.
/// </summary>
public class ForbiddenExceptionHandler(ILogger<ForbiddenExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not ForbiddenException ex)
            return false;

        // Los errores de negocio se registran como Warning: no son fallos del sistema.
        logger.LogWarning("Acceso denegado. errorCode={ErrorCode} path={Path}", ex.ErrorCode, httpContext.Request.Path.Value);

        await ErrorResponseWriter.WriteAsync(
            httpContext,
            StatusCodes.Status403Forbidden,
            "https://tools.ietf.org/html/rfc7231#section-6.5.3",
            "Forbidden",
            "El acceso está prohibido.",
            ex.ErrorCode,
            ex.Message,
            cancellationToken);

        return true;
    }
}

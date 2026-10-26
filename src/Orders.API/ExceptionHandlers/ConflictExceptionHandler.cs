using Orders.API.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace Orders.API.ExceptionHandlers;

/// <summary>
/// Convierte ConflictException en HTTP 409 (ORD-006, transición de estado no permitida).
/// Si la excepción no es de su tipo devuelve false y el framework prueba el siguiente handler.
/// </summary>
public class ConflictExceptionHandler(ILogger<ConflictExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not ConflictException ex)
            return false;

        // Los errores de negocio se registran como Warning: no son fallos del sistema.
        logger.LogWarning("Conflicto con el estado del recurso. errorCode={ErrorCode} path={Path}", ex.ErrorCode, httpContext.Request.Path.Value);

        await ErrorResponseWriter.WriteAsync(
            httpContext,
            StatusCodes.Status409Conflict,
            "https://tools.ietf.org/html/rfc7231#section-6.5.9",
            "Conflict",
            "No se puede modificar el estado.",
            ex.ErrorCode,
            ex.Message,
            cancellationToken);

        return true;
    }
}

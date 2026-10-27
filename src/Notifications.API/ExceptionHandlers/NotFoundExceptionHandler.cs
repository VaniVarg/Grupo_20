using Notifications.API.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace Notifications.API.ExceptionHandlers;

// Convierte NotFoundException en HTTP 404.
// Si la excepción no es de su tipo devuelve false y el framework prueba el siguiente handler.
public class NotFoundExceptionHandler(ILogger<NotFoundExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not NotFoundException ex)
            return false;

        // Los errores de negocio se registran como Warning: no son fallos del sistema.
        logger.LogWarning("Recurso no encontrado. errorCode={ErrorCode} path={Path}", ex.ErrorCode, httpContext.Request.Path.Value);

        await ErrorResponseWriter.WriteAsync(
            httpContext,
            StatusCodes.Status404NotFound,
            "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            "Not Found",
            "El recurso solicitado no fue encontrado.",
            ex.ErrorCode,
            ex.Message,
            cancellationToken);

        return true;
    }
}

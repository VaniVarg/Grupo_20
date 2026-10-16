using Cart.API.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace Cart.API.ExceptionHandlers;

/// <summary>
/// Convierte ValidationException en HTTP 400.
/// Si la excepción no es de su tipo devuelve false y el framework prueba el siguiente handler.
/// </summary>
public class ValidationExceptionHandler(ILogger<ValidationExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not ValidationException ex)
            return false;

        logger.LogWarning("Datos inválidos. errorCode={ErrorCode} path={Path}", ex.ErrorCode, httpContext.Request.Path.Value);

        await ErrorResponseWriter.WriteAsync(
            httpContext,
            StatusCodes.Status400BadRequest,
            "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            "Bad Request",
            "Los datos enviados son inválidos.",
            ex.ErrorCode,
            ex.Message,
            cancellationToken);

        return true;
    }
}
using Orders.API.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace Orders.API.ExceptionHandlers;

/// <summary>
/// Convierte BusinessRuleException en HTTP 422 (ORD-005, stock insuficiente).
/// Si la excepción no es de su tipo devuelve false y el framework prueba el siguiente handler.
/// </summary>
public class BusinessRuleExceptionHandler(ILogger<BusinessRuleExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not BusinessRuleException ex)
            return false;

        // Los errores de negocio se registran como Warning: no son fallos del sistema.
        logger.LogWarning("Regla de negocio violada. errorCode={ErrorCode} path={Path}", ex.ErrorCode, httpContext.Request.Path.Value);

        await ErrorResponseWriter.WriteAsync(
            httpContext,
            StatusCodes.Status422UnprocessableEntity,
            "https://tools.ietf.org/html/rfc4918#section-11.2",
            "Unprocessable Entity",
            "No se puede procesar la solicitud.",
            ex.ErrorCode,
            ex.Message,
            cancellationToken);

        return true;
    }
}

using Products.API.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace Products.API.ExceptionHandlers;

// Convierte BusinessRuleException en HTTP 409 (PRD-003 duplicado, PRD-004 órdenes activas).Si la excepción no es de su tipo devuelve false y el framework prueba el siguiente handler.

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
            StatusCodes.Status409Conflict,
            "https://tools.ietf.org/html/rfc7231#section-6.5.9",
            "Conflict",
            "La operación entra en conflicto con el estado actual del recurso.",
            ex.ErrorCode,
            ex.Message,
            cancellationToken);

        return true;
    }
}
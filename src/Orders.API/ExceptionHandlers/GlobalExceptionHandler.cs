using Orders.API.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace Orders.API.ExceptionHandlers;

/// <summary>
/// Red de seguridad: atrapa cualquier excepción que ningún otro handler haya reconocido
/// y responde 500 con ORD-007. Debe registrarse ULTIMO.
/// </summary>
public class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger,
    IHostEnvironment environment) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        // Los errores inesperados sí se registran como Error, con la excepción completa en el log.
        logger.LogError(exception, "Error inesperado. path={Path}", httpContext.Request.Path.Value);

        // Nunca se devuelve el stack trace. En Development mostramos el mensaje para depurar;
        // en Production, un texto genérico.
        var detail = environment.IsDevelopment()
            ? exception.Message
            : "Ocurrió un error inesperado.";

        await ErrorResponseWriter.WriteAsync(
            httpContext,
            StatusCodes.Status500InternalServerError,
            "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            "Internal Server Error",
            detail,
            ErrorCodes.ORD_007,
            "Error interno al procesar la orden.",
            cancellationToken);

        return true;
    }
}

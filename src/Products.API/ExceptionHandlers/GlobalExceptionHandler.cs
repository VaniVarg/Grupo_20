using Microsoft.AspNetCore.Diagnostics;
using Products.API.Exceptions;

namespace Products.API.ExceptionHandlers;

// Red de seguridad: atrapa cualquier excepción que ningún otro handler haya reconocido y responde 500 con PRD-005. Debe registrarse Último.
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

        // Nunca se devuelve el stack trace. En Development mostramos el mensaje para depurar; y en Production, un texto genérico.
        var detail = environment.IsDevelopment()
            ? exception.Message
            : "Ocurrió un error inesperado.";

        await ErrorResponseWriter.WriteAsync(
            httpContext,
            StatusCodes.Status500InternalServerError,
            "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            "Internal Server Error",
            detail,
            ErrorCodes.PRD_005,
            "Error interno al procesar el producto.",
            cancellationToken);

        return true;
    }
}
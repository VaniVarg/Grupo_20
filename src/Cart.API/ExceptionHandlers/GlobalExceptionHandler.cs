using Cart.API.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace Cart.API.ExceptionHandlers;

/// <summary>
/// Red de seguridad: atrapa cualquier excepción que ningún otro handler haya reconocido
/// y responde 500 con CRT-005. Debe registrarse ÚLTIMO.
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
        logger.LogError(exception, "Error inesperado. path={Path}", httpContext.Request.Path.Value);

        var detail = environment.IsDevelopment()
            ? exception.Message
            : "Ocurrió un error inesperado.";

        await ErrorResponseWriter.WriteAsync(
            httpContext,
            StatusCodes.Status500InternalServerError,
            "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            "Internal Server Error",
            detail,
            ErrorCodes.CRT_005,
            "Error interno al procesar el carrito.",
            cancellationToken);

        return true;
    }
}

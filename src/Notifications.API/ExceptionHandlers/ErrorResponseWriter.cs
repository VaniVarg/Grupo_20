using Notifications.API.Middleware;
namespace Notifications.API.ExceptionHandlers;

// Escribe la respuesta de error con el formato de la consigna: los campos estándar de
// Problem Details, errorCode, errorMessage y el correlationId del request.

public static class ErrorResponseWriter
{
    public static async Task WriteAsync(
        HttpContext context,
        int status,
        string type,
        string title,
        string detail,
        string errorCode,
        string errorMessage,
        CancellationToken cancellationToken)
    {
        context.Response.StatusCode = status;

        // Campo extra que pide la consigna: permite buscar este error en los logs de todos los servicios.
        context.Items.TryGetValue(CorrelationIdMiddleware.ItemKey, out var correlationId);

        await context.Response.WriteAsJsonAsync(new
        {
            type,
            title,
            status,
            detail,
            instance = context.Request.Path.Value,
            errorCode,
            errorMessage,
            correlationId = correlationId as string
        }, cancellationToken: cancellationToken);
    }
}



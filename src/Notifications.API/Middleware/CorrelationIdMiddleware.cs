namespace Notifications.API.Middleware;

// Asigna un identificador de correlación a cada request y lo deja disponible para todo el
// resto del código. Con ese id se puede seguir un mismo pedido a través de varios servicios.

public class CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
{
    public const string HeaderName = "X-Correlation-Id";
    public const string ItemKey = "CorrelationId";

    public async Task InvokeAsync(HttpContext context)
    {
        // Si el request ya trae un id (porque lo llama otro servicio) se reutiliza;
        // si no, se genera uno nuevo. Se descarta un valor vacío o desmesurado.
        var correlationId = context.Request.Headers[HeaderName].FirstOrDefault();
        if (string.IsNullOrWhiteSpace(correlationId) || correlationId.Length > 64)
            correlationId = Guid.NewGuid().ToString("N");

        // Queda guardado en el HttpContext para que lo lean los handlers de errores y los clientes HTTP.
        context.Items[ItemKey] = correlationId;

        // Se devuelve en un header de la respuesta. Se usa OnStarting porque el handler de errores
        // limpia la respuesta: así el header se agrega justo antes de enviarla, siempre.
        context.Response.OnStarting(() =>
        {
            context.Response.Headers[HeaderName] = correlationId;
            return Task.CompletedTask;
        });

        // El scope agrega CorrelationId a TODOS los logs que se escriban durante este request.
        using (logger.BeginScope(new Dictionary<string, object> { [ItemKey] = correlationId }))
        {
            await next(context);
        }
    }
}

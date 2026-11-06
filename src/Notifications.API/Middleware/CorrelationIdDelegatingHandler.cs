namespace Notifications.API.Middleware;

// Se engancha a los HttpClient: antes de cada llamada saliente a otro servicio copia el
// Correlation ID del request actual en el header X-Correlation-Id. Así el otro servicio
// reutiliza el mismo id y todo el recorrido queda con un único identificador.

public class CorrelationIdDelegatingHandler(IHttpContextAccessor httpContextAccessor) : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (httpContextAccessor.HttpContext is { } context
            && context.Items.TryGetValue(CorrelationIdMiddleware.ItemKey, out var value)
            && value is string correlationId
            && !request.Headers.Contains(CorrelationIdMiddleware.HeaderName))
        {
            request.Headers.Add(CorrelationIdMiddleware.HeaderName, correlationId);
        }

        return base.SendAsync(request, cancellationToken);
    }
}

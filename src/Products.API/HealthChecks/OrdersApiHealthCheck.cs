using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Products.API.HealthChecks;

// Comprueba que Orders.API esté accesible. Si no responde, el estado es Degraded (el servicio
// sigue funcionando a medias) y no Unhealthy.
public class OrdersApiHealthCheck(HttpClient httpClient) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Alcanza con que el servicio conteste algo (aunque sea un 404): significa que está vivo.
            using var response = await httpClient.GetAsync("/", cancellationToken);
            return HealthCheckResult.Healthy("Orders.API responde.");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Degraded("Orders.API no responde.", ex);
        }
    }
}

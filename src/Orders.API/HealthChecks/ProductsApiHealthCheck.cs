using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Orders.API.HealthChecks;

/// <summary>
/// Comprueba que Products.API esté accesible. Si no responde, el estado es Degraded (el servicio
/// sigue funcionando a medias) y no Unhealthy.
/// </summary>
public class ProductsApiHealthCheck(HttpClient httpClient) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Alcanza con que el servicio conteste algo (aunque sea un 404): significa que está vivo.
            using var response = await httpClient.GetAsync("/", cancellationToken);
            return HealthCheckResult.Healthy("Products.API responde.");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Degraded("Products.API no responde.", ex);
        }
    }
}

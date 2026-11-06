using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Products.API.HealthChecks;

// Escribe el resultado de los health checks como JSON, con el estado general
// (Healthy, Degraded o Unhealthy) y el detalle de cada chequeo.
public static class HealthCheckResponseWriter
{
    public static Task WriteAsync(HttpContext context, HealthReport report)
    {
        var payload = new
        {
            status = report.Status.ToString(),
            totalDurationMs = Math.Round(report.TotalDuration.TotalMilliseconds, 1),
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                description = e.Value.Description,
                durationMs = Math.Round(e.Value.Duration.TotalMilliseconds, 1)
            })
        };

        return context.Response.WriteAsJsonAsync(payload);
    }
}

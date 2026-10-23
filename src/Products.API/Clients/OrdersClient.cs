using System.Net.Http.Json;

namespace Products.API.Clients;

public class OrdersClient(HttpClient httpClient, ILogger<OrdersClient> logger) : IOrdersClient
{
    // Estados que la consigna considera "activos" (los mismos textos que usa Orders.API).
    private static readonly string[] EstadosActivos = ["Pendiente", "Confirmada"];

    public async Task<bool> HasActiveOrdersAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        // Orders.API no tiene un filtro por producto, así que pedimos todas las órdenes y buscamos acá. Para este TP alcanza; en un sistema real Orders expondría ese filtro.
        var response = await httpClient.GetAsync("api/orders", cancellationToken);

        // Siempre se revisa el status HTTP ANTES de leer el body.
        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("Orders.API respondió {StatusCode} al listar órdenes", (int)response.StatusCode);
            throw new HttpRequestException($"Orders.API respondió {(int)response.StatusCode}.");
        }

        var orders = await response.Content.ReadFromJsonAsync<List<OrderInfo>>(cancellationToken) ?? [];

        return orders.Any(o =>
            EstadosActivos.Contains(o.Estado) &&
            o.Items.Any(i => i.ProductoId == productId));
    }
}

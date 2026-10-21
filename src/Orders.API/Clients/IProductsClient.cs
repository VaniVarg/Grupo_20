namespace Orders.API.Clients;

/// <summary>
/// Cliente HTTP de Products.API. Los Services dependen de esta interfaz, no de HttpClient.
/// </summary>
public interface IProductsClient
{
    /// <summary>Devuelve el producto o null si no existe (HTTP 404).</summary>
    Task<ProductInfo?> GetProductAsync(Guid id, CancellationToken cancellationToken = default);
}

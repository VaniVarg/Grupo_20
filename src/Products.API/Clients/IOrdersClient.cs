namespace Products.API.Clients;

// Cliente HTTP de Orders.API. El Service depende de esta interfaz, no de HttpClient.
public interface IOrdersClient
{
    // True si hay órdenes Pendiente o Confirmada que incluyen el producto.
    Task<bool> HasActiveOrdersAsync(Guid productId, CancellationToken cancellationToken = default);
}

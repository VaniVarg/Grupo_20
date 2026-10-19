using Orders.API.DTOs;

namespace Orders.API.Services;

/// <summary>
/// Lógica de negocio de órdenes.
/// </summary>
public interface IOrderService
{
    Task<OrderResponse> CreateAsync(CreateOrderRequest request);
}

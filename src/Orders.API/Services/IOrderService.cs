using Orders.API.DTOs;

namespace Orders.API.Services;

/// <summary>
/// Lógica de negocio de órdenes.
/// </summary>
public interface IOrderService
{
    Task<OrderResponse> CreateAsync(CreateOrderRequest request);
    Task<OrderResponse> GetByIdAsync(Guid id);
    Task<IEnumerable<OrderResponse>> GetAllAsync(Guid? usuarioId);
    Task<UpdateOrderStatusResponse> UpdateStatusAsync(Guid id, UpdateOrderStatusRequest request);
}

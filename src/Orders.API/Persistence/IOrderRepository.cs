using Orders.API.Models;

namespace Orders.API.Persistence;

/// <summary>
/// Contrato de acceso a datos de órdenes. El Service solo conoce esta interfaz.
/// </summary>
public interface IOrderRepository
{
    Task AddAsync(Order order);
    Task<Order?> GetByIdAsync(Guid id);
    Task<IEnumerable<Order>> GetAllAsync(Guid? usuarioId);
    Task UpdateAsync(Order order);
}

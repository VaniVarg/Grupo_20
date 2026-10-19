using System.Collections.Concurrent;
using Orders.API.Models;

namespace Orders.API.Persistence;

/// <summary>
/// Implementación temporal en memoria (los datos se pierden al reiniciar).
/// </summary>
public class InMemoryOrderRepository : IOrderRepository
{
    private readonly ConcurrentDictionary<Guid, Order> _orders = new();

    public Task AddAsync(Order order)
    {
        _orders[order.Id] = order;
        return Task.CompletedTask;
    }

    public Task<Order?> GetByIdAsync(Guid id)
    {
        _orders.TryGetValue(id, out var order);
        return Task.FromResult<Order?>(order);
    }

    public Task<IEnumerable<Order>> GetAllAsync(Guid? usuarioId)
    {
        IEnumerable<Order> query = _orders.Values;

        // Filtro opcional ?usuarioId=
        if (usuarioId.HasValue)
            query = query.Where(o => o.UsuarioId == usuarioId.Value);

        return Task.FromResult<IEnumerable<Order>>(query.OrderByDescending(o => o.FechaCreacion).ToList());
    }
}

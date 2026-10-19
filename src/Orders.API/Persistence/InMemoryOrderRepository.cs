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
}

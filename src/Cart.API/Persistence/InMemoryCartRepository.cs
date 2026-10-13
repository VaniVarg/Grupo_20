using System.Collections.Concurrent;
using Cart.API.Models;

namespace Cart.API.Persistence;

/// <summary>
/// Implementación temporal en memoria (los datos se pierden al reiniciar).
/// La clave del diccionario es el UsuarioId: hay un carrito por usuario.
/// </summary>
public class InMemoryCartRepository : ICartRepository
{
    private readonly ConcurrentDictionary<Guid, ShoppingCart> _carts = new();

    public Task<ShoppingCart?> GetByUserIdAsync(Guid userId)
    {
        _carts.TryGetValue(userId, out var cart);
        return Task.FromResult<ShoppingCart?>(cart);
    }

    public Task SaveAsync(ShoppingCart cart)
    {
        _carts[cart.UsuarioId] = cart;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid userId)
    {
        _carts.TryRemove(userId, out _);
        return Task.CompletedTask;
    }
}

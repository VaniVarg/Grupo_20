using Cart.API.Models;

namespace Cart.API.Persistence;

/// <summary>
/// Contrato de acceso a datos del carrito. El Service solo conoce esta interfaz.
/// </summary>
public interface ICartRepository
{
    Task<ShoppingCart?> GetByUserIdAsync(Guid userId);
    Task SaveAsync(ShoppingCart cart);
    Task DeleteAsync(Guid userId);
}

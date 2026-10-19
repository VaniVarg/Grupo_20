using Cart.API.DTOs;

namespace Cart.API.Services;

/// <summary>
/// Lógica de negocio del carrito.
/// </summary>
public interface ICartService
{
    Task<CartResponse> GetCartAsync(Guid userId);
    Task<CartResponse> AddItemAsync(Guid userId, AddCartItemRequest request);
    Task<CartResponse> UpdateItemAsync(Guid userId, Guid productId, UpdateCartItemRequest request);
    Task RemoveItemAsync(Guid userId, Guid productId);
    Task ClearCartAsync(Guid userId);
}
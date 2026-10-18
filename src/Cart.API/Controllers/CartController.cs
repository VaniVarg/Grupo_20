using Cart.API.DTOs;
using Cart.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cart.API.Controllers;

/// <summary>
/// Gestión del carrito de compras de un usuario.
/// </summary>
[ApiController]
[Route("api/cart")]
[Produces("application/json")]
public class CartController(ICartService service) : ControllerBase
{
    /// <summary>Obtiene el carrito de un usuario.</summary>
    [HttpGet("{userId}")]
    public async Task<ActionResult<CartResponse>> GetCart(Guid userId)
    {
        return Ok(await service.GetCartAsync(userId));
    }

    /// <summary>Agrega un producto al carrito (si ya estaba, suma la cantidad).</summary>
    [HttpPost("{userId}/items")]
    public async Task<ActionResult<CartResponse>> AddItem(Guid userId, [FromBody] AddCartItemRequest request)
    {
        return Ok(await service.AddItemAsync(userId, request));
    }

    /// <summary>Actualiza la cantidad de un producto del carrito.</summary>
    [HttpPut("{userId}/items/{productId}")]
    public async Task<ActionResult<CartResponse>> UpdateItem(
        Guid userId, Guid productId, [FromBody] UpdateCartItemRequest request)
    {
        return Ok(await service.UpdateItemAsync(userId, productId, request));
    }

    /// <summary>Quita un producto del carrito.</summary>
    [HttpDelete("{userId}/items/{productId}")]
    public async Task<IActionResult> RemoveItem(Guid userId, Guid productId)
    {
        await service.RemoveItemAsync(userId, productId);
        return NoContent();
    }

    /// <summary>Vacía el carrito completo.</summary>
    [HttpDelete("{userId}")]
    public async Task<IActionResult> ClearCart(Guid userId)
    {
        await service.ClearCartAsync(userId);
        return NoContent();
    }
}
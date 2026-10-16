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
}
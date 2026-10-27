using Cart.API.Exceptions;
using Cart.API.Swagger;
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
    [ProducesResponseType(typeof(CartResponse), StatusCodes.Status200OK)]
    [ErrorResponse(404, ErrorCodes.CRT_001, "Carrito no encontrado.")]
    [ErrorResponse(500, ErrorCodes.CRT_005, "Error interno al procesar el carrito.")]
    [HttpGet("{userId}")]
    public async Task<ActionResult<CartResponse>> GetCart(Guid userId)
    {
        return Ok(await service.GetCartAsync(userId));
    }

    /// <summary>Agrega un producto al carrito (si ya estaba, suma la cantidad).</summary>
    [ProducesResponseType(typeof(CartResponse), StatusCodes.Status200OK)]
    [ErrorResponse(400, ErrorCodes.CRT_004, "Cantidad inválida: debe ser mayor a 0.")]
    [ErrorResponse(404, ErrorCodes.CRT_002, "Producto no encontrado.")]
    [ErrorResponse(422, ErrorCodes.CRT_003, "Stock insuficiente. Disponible: 10, solicitado: 20.")]
    [ErrorResponse(500, ErrorCodes.CRT_005, "Error interno al procesar el carrito.")]
    [HttpPost("{userId}/items")]
    public async Task<ActionResult<CartResponse>> AddItem(Guid userId, [FromBody] AddCartItemRequest request)
    {
        return Ok(await service.AddItemAsync(userId, request));
    }

    /// <summary>Actualiza la cantidad de un producto del carrito.</summary>
    [ProducesResponseType(typeof(CartResponse), StatusCodes.Status200OK)]
    [ErrorResponse(400, ErrorCodes.CRT_004, "Cantidad inválida: debe ser mayor a 0.")]
    [ErrorResponse(404, ErrorCodes.CRT_001, "El producto no está en el carrito.")]
    [ErrorResponse(404, ErrorCodes.CRT_002, "Producto no encontrado.")]
    [ErrorResponse(422, ErrorCodes.CRT_003, "Stock insuficiente. Disponible: 10, solicitado: 20.")]
    [ErrorResponse(500, ErrorCodes.CRT_005, "Error interno al procesar el carrito.")]
    [HttpPut("{userId}/items/{productId}")]
    public async Task<ActionResult<CartResponse>> UpdateItem(
        Guid userId, Guid productId, [FromBody] UpdateCartItemRequest request)
    {
        return Ok(await service.UpdateItemAsync(userId, productId, request));
    }

    /// <summary>Quita un producto del carrito.</summary>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ErrorResponse(404, ErrorCodes.CRT_001, "El producto no está en el carrito.")]
    [ErrorResponse(500, ErrorCodes.CRT_005, "Error interno al procesar el carrito.")]
    [HttpDelete("{userId}/items/{productId}")]
    public async Task<IActionResult> RemoveItem(Guid userId, Guid productId)
    {
        await service.RemoveItemAsync(userId, productId);
        return NoContent();
    }

    /// <summary>Vacía el carrito completo.</summary>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ErrorResponse(404, ErrorCodes.CRT_001, "Carrito no encontrado.")]
    [ErrorResponse(500, ErrorCodes.CRT_005, "Error interno al procesar el carrito.")]
    [HttpDelete("{userId}")]
    public async Task<IActionResult> ClearCart(Guid userId)
    {
        await service.ClearCartAsync(userId);
        return NoContent();
    }
}

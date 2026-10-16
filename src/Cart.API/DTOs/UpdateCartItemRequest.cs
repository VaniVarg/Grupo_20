namespace Cart.API.DTOs;

/// <summary>
/// Cuerpo del PUT /api/cart/{userId}/items/{productId}.
/// </summary>
public class UpdateCartItemRequest
{
    /// <summary>Nueva cantidad (mayor a 0).</summary>
    public int Cantidad { get; set; }
}
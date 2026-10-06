namespace Cart.API.DTOs;

/// <summary>
/// Cuerpo del POST /api/cart/{userId}/items.
/// La cantidad NO lleva [Range] a propósito: una cantidad menor o igual a 0 debe
/// devolver el código CRT-004, y eso lo valida la capa de servicios.
/// </summary>
public class AddCartItemRequest
{
    /// <summary>Producto a agregar.</summary>
    public Guid ProductoId { get; set; }

    /// <summary>Cantidad a agregar (mayor a 0).</summary>
    public int Cantidad { get; set; }
}
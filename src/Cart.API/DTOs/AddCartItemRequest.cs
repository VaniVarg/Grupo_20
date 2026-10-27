namespace Cart.API.DTOs;

/// <summary>
/// Cuerpo del POST /api/cart/{userId}/items.
/// La cantidad NO lleva [Range] a propósito: una cantidad menor o igual a 0 debe
/// devolver el código CRT-004, y eso lo valida la capa de servicios.
/// </summary>
public class AddCartItemRequest
{
    /// <summary>Producto a agregar.</summary>
    /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
    public Guid ProductoId { get; set; }

    /// <summary>Cantidad a agregar (mayor a 0).</summary>
    /// <example>2</example>
    public int Cantidad { get; set; }
}
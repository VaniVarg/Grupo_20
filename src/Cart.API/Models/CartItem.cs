namespace Cart.API.Models;

/// <summary>
/// Producto dentro del carrito, con su cantidad.
/// </summary>
public class CartItem
{
    /// <summary>Producto referenciado (vive en Products.API).</summary>
    public Guid ProductoId { get; set; }

    /// <summary>Cantidad, mayor a 0.</summary>
    public int Cantidad { get; set; }
}
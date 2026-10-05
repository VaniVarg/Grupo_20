namespace Orders.API.Models;

/// <summary>
/// Línea de una orden: un producto con su cantidad y el precio al momento de la compra.
/// </summary>
public class OrderItem
{
    /// <summary>Producto referenciado (vive en Products.API).</summary>
    public Guid ProductoId { get; set; }

    /// <summary>Cantidad pedida, mayor a 0.</summary>
    public int Cantidad { get; set; }

    /// <summary>Precio del producto capturado al crear la orden.</summary>
    public decimal PrecioUnitario { get; set; }
}

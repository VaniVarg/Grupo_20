namespace Orders.API.DTOs;

public class OrderItemResponse
{
    /// <summary>Producto incluido en la orden.</summary>
    /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
    public Guid ProductoId { get; set; }
    /// <summary>Cantidad pedida.</summary>
    /// <example>2</example>
    public int Cantidad { get; set; }
    /// <summary>Precio del producto al momento de la compra.</summary>
    /// <example>1500.00</example>
    public decimal PrecioUnitario { get; set; }
}

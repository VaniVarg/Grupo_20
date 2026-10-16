namespace Orders.API.Models;

/// <summary>
/// Orden de compra de un usuario.
/// </summary>
public class Order
{
    public Guid Id { get; set; }

    /// <summary>Usuario que realizó la orden (vive en Users.API).</summary>
    public Guid UsuarioId { get; set; }

    public List<OrderItem> Items { get; set; } = [];

    /// <summary>Suma de Cantidad x PrecioUnitario, calculada al crear la orden.</summary>
    public decimal Total { get; set; }

    /// <summary>Ver <see cref="OrderStatus"/>. Toda orden nueva arranca en Pendiente.</summary>
    public string Estado { get; set; } = OrderStatus.Pendiente;

    public DateTime FechaCreacion { get; set; }

    /// <summary>Se completa cada vez que cambia el estado.</summary>
    public DateTime? FechaActualizacion { get; set; }
}

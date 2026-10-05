namespace Orders.API.DTOs;

/// <summary>Detalle de una orden.</summary>
public class OrderResponse
{
    public Guid Id { get; set; }
    public Guid UsuarioId { get; set; }
    public List<OrderItemResponse> Items { get; set; } = [];
    public decimal Total { get; set; }
    public string Estado { get; set; } = string.Empty;
    public DateTime FechaCreacion { get; set; }
}

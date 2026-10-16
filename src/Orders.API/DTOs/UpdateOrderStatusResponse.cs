namespace Orders.API.DTOs;

/// <summary>Respuesta del PUT /api/orders/{id}/status.</summary>
public class UpdateOrderStatusResponse
{
    public Guid Id { get; set; }
    public string Estado { get; set; } = string.Empty;
    public DateTime FechaActualizacion { get; set; }
}

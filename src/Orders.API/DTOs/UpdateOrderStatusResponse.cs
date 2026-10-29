namespace Orders.API.DTOs;

/// <summary>Respuesta del PUT /api/orders/{id}/status.</summary>
public class UpdateOrderStatusResponse
{
    /// <summary>Identificador de la orden.</summary>
    /// <example>f1e2d3c4-0000-0000-0000-aabbccddeeff</example>
    public Guid Id { get; set; }
    /// <summary>Nuevo estado.</summary>
    /// <example>Confirmada</example>
    public string Estado { get; set; } = string.Empty;
    /// <summary>Fecha del cambio de estado en UTC.</summary>
    /// <example>2026-10-28T12:00:00Z</example>
    public DateTime FechaActualizacion { get; set; }
}

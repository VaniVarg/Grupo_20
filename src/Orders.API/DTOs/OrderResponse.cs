namespace Orders.API.DTOs;

/// <summary>Detalle de una orden.</summary>
public class OrderResponse
{
    /// <summary>Identificador único de la orden.</summary>
    /// <example>f1e2d3c4-0000-0000-0000-aabbccddeeff</example>
    public Guid Id { get; set; }
    /// <summary>Usuario que realizó la orden.</summary>
    /// <example>a1b2c3d4-0000-0000-0000-111122223333</example>
    public Guid UsuarioId { get; set; }
    public List<OrderItemResponse> Items { get; set; } = [];
    /// <summary>Suma de cantidad por precio unitario.</summary>
    /// <example>3000.00</example>
    public decimal Total { get; set; }
    /// <summary>Estado actual: Pendiente, Confirmada, Enviada, Entregada o Cancelada.</summary>
    /// <example>Pendiente</example>
    public string Estado { get; set; } = string.Empty;
    /// <summary>Fecha de creación en UTC.</summary>
    /// <example>2026-10-28T12:00:00Z</example>
    public DateTime FechaCreacion { get; set; }
}

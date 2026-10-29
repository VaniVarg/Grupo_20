using System.ComponentModel.DataAnnotations;

namespace Orders.API.DTOs;

/// <summary>Item de una orden dentro del POST /api/orders.</summary>
public class OrderItemRequest
{
    /// <summary>ID del producto a comprar.</summary>
    /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
    public Guid ProductoId { get; set; }

    /// <summary>Cantidad (mayor a 0).</summary>
    /// <example>2</example>
    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0.")]
    public int Cantidad { get; set; }
}

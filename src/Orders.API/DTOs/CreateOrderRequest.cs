using System.ComponentModel.DataAnnotations;

namespace Orders.API.DTOs;

/// <summary>Cuerpo del POST /api/orders.</summary>
public class CreateOrderRequest
{
    /// <summary>Usuario que realiza la orden.</summary>
    public Guid UsuarioId { get; set; }

    /// <summary>Productos a comprar (al menos uno).</summary>
    [Required(ErrorMessage = "La lista de items es obligatoria.")]
    [MinLength(1, ErrorMessage = "La orden debe tener al menos un item.")]
    public List<OrderItemRequest> Items { get; set; } = [];
}

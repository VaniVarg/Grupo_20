using System.ComponentModel.DataAnnotations;

namespace Orders.API.DTOs;

/// <summary>Cuerpo del PUT /api/orders/{id}/status.</summary>
public class UpdateOrderStatusRequest
{
    /// <summary>Nuevo estado: Pendiente, Confirmada, Enviada, Entregada o Cancelada.</summary>
    /// <example>Confirmada</example>
    [Required(ErrorMessage = "El estado es obligatorio.")]
    public string Estado { get; set; } = string.Empty;
}

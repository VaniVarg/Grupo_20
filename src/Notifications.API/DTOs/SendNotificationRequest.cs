using System.ComponentModel.DataAnnotations;

namespace Notifications.API.DTOs;

// <summary>Cuerpo del POST /api/notifications/send.</summary>
public class SendNotificationRequest
{
    // <summary>Usuario destinatario.</summary>
    public Guid UsuarioId { get; set; }

    // <summary>Texto del mensaje.</summary>
    [Required(ErrorMessage = "El mensaje es obligatorio.")]
    [StringLength(500, ErrorMessage = "El mensaje no puede superar los 500 caracteres.")]
    public string Mensaje { get; set; } = string.Empty;

    // <summary>Email, Push o SMS. El tipo se valida en el Service (NTF-002).</summary>
    [Required(ErrorMessage = "El tipo es obligatorio.")]
    public string Tipo { get; set; } = string.Empty;
}
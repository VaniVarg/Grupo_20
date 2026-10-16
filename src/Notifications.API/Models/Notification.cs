namespace Notifications.API.Models;

// Notificación dirigida a un usuario.

public class Notification
{
    public Guid Id { get; set; }

    // <summary>Usuario destinatario (vive en Users.API).</summary>
    public Guid UsuarioId { get; set; }

    // <summary>Texto del mensaje (requerido, máx. 500 caracteres).</summary>
    public string Mensaje { get; set; } = string.Empty;

    // <summary>Ver <see cref="NotificationType"/>.</summary>
    public string Tipo { get; set; } = string.Empty;

    // <summary>Ver <see cref="NotificationStatus"/>.</summary>
    public string Estado { get; set; } = NotificationStatus.Pendiente;

    // <summary>Asignada automáticamente al registrar la notificación.</summary>
    public DateTime FechaEnvio { get; set; }
}
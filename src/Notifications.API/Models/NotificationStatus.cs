namespace Notifications.API.Models;

// Estados de una notificación. El envío es simulado, así que normalmente pasa a Enviada.
public static class NotificationStatus
{
    public const string Pendiente = "Pendiente";
    public const string Enviada = "Enviada";
    public const string Fallida = "Fallida";
}
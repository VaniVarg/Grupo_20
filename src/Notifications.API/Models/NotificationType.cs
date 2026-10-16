namespace Notifications.API.Models;

// Tipos de notificación permitidos. Cualquier otro valor es inválido (NTF-002).

public static class NotificationType
{
    public const string Email = "Email";
    public const string Push = "Push";
    public const string Sms = "SMS";

    public static readonly string[] All = [Email, Push, Sms];
}
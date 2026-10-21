using Notifications.API.DTOs;

namespace Notifications.API.Services;

// Lógica de negocio de notificaciones.
public interface INotificationService
{
    Task<NotificationResponse> SendAsync(SendNotificationRequest request);
    Task<IEnumerable<NotificationResponse>> GetByUserAsync(Guid userId);
}
using Notifications.API.Models;

namespace Notifications.API.Persistence;

// Contrato de acceso a datos de notificaciones. El Service solo conoce esta interfaz.

public interface INotificationRepository
{
    Task AddAsync(Notification notification);
    Task<List<Notification>> GetByUserIdAsync(Guid userId);
}
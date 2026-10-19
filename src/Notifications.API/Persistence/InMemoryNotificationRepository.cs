using System.Collections.Concurrent;
using Notifications.API.Models;

namespace Notifications.API.Persistence;

// Implementación temporal en memoria (los datos se pierden al reiniciar).

public class InMemoryNotificationRepository : INotificationRepository
{
    private readonly ConcurrentDictionary<Guid, Notification> _notifications = new();

    public Task AddAsync(Notification notification)
    {
        _notifications[notification.Id] = notification;
        return Task.CompletedTask;
    }

    public Task<List<Notification>> GetByUserIdAsync(Guid userId)
    {
        var list = _notifications.Values
            .Where(n => n.UsuarioId == userId)
            .OrderByDescending(n => n.FechaEnvio)
            .ToList();

        return Task.FromResult(list);
    }
}
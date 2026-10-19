using Notifications.API.Clients;
using Notifications.API.DTOs;
using Notifications.API.Exceptions;
using Notifications.API.Models;
using Notifications.API.Persistence;

namespace Notifications.API.Services;

public class NotificationService(
    INotificationRepository repository,
    IUsersClient usersClient,
    ILogger<NotificationService> logger) : INotificationService
{
    public async Task<NotificationResponse> SendAsync(SendNotificationRequest request)
    {
        RequestValidator.Validate(request, ErrorCodes.NTF_002, "Los datos de la notificación son inválidos.");

        // El tipo se compara sin distinguir mayúsculas y se guarda con su valor canónico ("push" -> "Push").
        var tipo = NotificationType.All
            .FirstOrDefault(t => t.Equals(request.Tipo.Trim(), StringComparison.OrdinalIgnoreCase))
            ?? throw new ValidationException(
                ErrorCodes.NTF_002,
                $"Tipo de notificación inválido. Valores permitidos: {string.Join(", ", NotificationType.All)}.");

        // Llamada HTTP a Users.API: el destinatario tiene que existir (NTF-001).
        if (!await usersClient.ExistsAsync(request.UsuarioId))
            throw new NotFoundException(ErrorCodes.NTF_001, "Usuario destinatario no encontrado.");

        var notification = new Notification
        {
            Id = Guid.NewGuid(),
            UsuarioId = request.UsuarioId,
            Mensaje = request.Mensaje.Trim(),
            Tipo = tipo,
            // El envío es simulado: la notificación se marca como enviada directamente.
            Estado = NotificationStatus.Enviada,
            FechaEnvio = DateTime.UtcNow
        };

        await repository.AddAsync(notification);

        logger.LogInformation("Notificación {Tipo} enviada (simulada) al usuario {UsuarioId}", tipo, request.UsuarioId);

        return ToResponse(notification);
    }

    public async Task<IEnumerable<NotificationResponse>> GetByUserAsync(Guid userId)
    {
        var notifications = await repository.GetByUserIdAsync(userId);

        if (notifications.Count == 0)
            throw new NotFoundException(ErrorCodes.NTF_003, "No se encontraron notificaciones para el usuario.");

        return notifications.Select(ToResponse).ToList();
    }

    private static NotificationResponse ToResponse(Notification n) => new()
    {
        Id = n.Id,
        UsuarioId = n.UsuarioId,
        Mensaje = n.Mensaje,
        Tipo = n.Tipo,
        Estado = n.Estado,
        FechaEnvio = n.FechaEnvio
    };
}
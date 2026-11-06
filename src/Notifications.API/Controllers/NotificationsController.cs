using Notifications.API.Exceptions;
using Notifications.API.Swagger;
using Microsoft.AspNetCore.Mvc;
using Notifications.API.DTOs;
using Notifications.API.Services;

namespace Notifications.API.Controllers;

// Registro y consulta de notificaciones.
[ApiController]
[Route("api/notifications")]
[Produces("application/json")]
public class NotificationsController(INotificationService service) : ControllerBase
{
    // Registra una notificación y simula su envío.
    [ProducesResponseType(typeof(NotificationResponse), StatusCodes.Status201Created)]
    [ErrorResponse(400, ErrorCodes.NTF_002, "Tipo de notificación inválido. Valores permitidos: Email, Push, SMS.")]
    [ErrorResponse(404, ErrorCodes.NTF_001, "El usuario destinatario no fue encontrado.")]
    [ErrorResponse(500, ErrorCodes.NTF_004, "Error interno al procesar la notificación.")]
    [HttpPost("send")]
    public async Task<ActionResult<NotificationResponse>> Send([FromBody] SendNotificationRequest request)
    {
        var created = await service.SendAsync(request);
        return Created($"/api/notifications/{created.UsuarioId}", created);
    }

    // Lista las notificaciones de un usuario (las más recientes primero).
    [ProducesResponseType(typeof(IEnumerable<NotificationResponse>), StatusCodes.Status200OK)]
    [ErrorResponse(404, ErrorCodes.NTF_003, "No se encontraron notificaciones para el usuario.")]
    [ErrorResponse(500, ErrorCodes.NTF_004, "Error interno al procesar la notificación.")]
    [HttpGet("{userId}")]
    public async Task<ActionResult<IEnumerable<NotificationResponse>>> GetByUser(Guid userId)
    {
        return Ok(await service.GetByUserAsync(userId));
    }
}
using Notifications.API.Exceptions;
using Notifications.API.Swagger;
using Microsoft.AspNetCore.Mvc;
using Notifications.API.DTOs;
using Notifications.API.Services;

namespace Notifications.API.Controllers;

/// <summary>
/// Registro y consulta de notificaciones.
/// </summary>
[ApiController]
[Route("api/notifications")]
[Produces("application/json")]
public class NotificationsController(INotificationService service) : ControllerBase
{
    /// <summary>Registra una notificación y simula su envío.</summary>
    [ProducesResponseType(typeof(NotificationResponse), StatusCodes.Status201Created)]
    [ErrorResponse(400, ErrorCodes.NTF_002, "Tipo de notificación inválido. Valores permitidos: Email, Push, SMS.")]
    [ErrorResponse(404, ErrorCodes.NTF_001, "Usuario destinatario no encontrado.")]
    [ErrorResponse(500, ErrorCodes.NTF_004, "Error interno al procesar la notificación.")]
    [HttpPost("send")]
    public async Task<ActionResult<NotificationResponse>> Send([FromBody] SendNotificationRequest request)
    {
        var created = await service.SendAsync(request);
        return Created($"/api/notifications/{created.UsuarioId}", created);
    }

    /// <summary>Lista las notificaciones de un usuario (las más recientes primero).</summary>
    [ProducesResponseType(typeof(IEnumerable<NotificationResponse>), StatusCodes.Status200OK)]
    [ErrorResponse(404, ErrorCodes.NTF_003, "No se encontraron notificaciones para el usuario.")]
    [ErrorResponse(500, ErrorCodes.NTF_004, "Error interno al procesar la notificación.")]
    [HttpGet("{userId}")]
    public async Task<ActionResult<IEnumerable<NotificationResponse>>> GetByUser(Guid userId)
    {
        return Ok(await service.GetByUserAsync(userId));
    }
}

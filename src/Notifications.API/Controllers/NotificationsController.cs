using Microsoft.AspNetCore.Mvc;
using Notifications.API.DTOs;
using Notifications.API.Services;

namespace Notifications.API.Controllers;

//Registro y consulta de notificaciones.

[ApiController]
[Route("api/notifications")]
[Produces("application/json")]
public class NotificationsController(INotificationService service) : ControllerBase
{
    //Registra una notificación y simula su envío.
    [HttpPost("send")]
    public async Task<ActionResult<NotificationResponse>> Send([FromBody] SendNotificationRequest request)
    {
        var created = await service.SendAsync(request);
        return Created($"/api/notifications/{created.UsuarioId}", created);
    }

    //Lista las notificaciones de un usuario (las más recientes primero).
    [HttpGet("{userId}")]
    public async Task<ActionResult<IEnumerable<NotificationResponse>>> GetByUser(Guid userId)
    {
        return Ok(await service.GetByUserAsync(userId));
    }
}
using Microsoft.AspNetCore.Mvc;
using Users.API.DTOs;
using Users.API.Services;

namespace Users.API.Controllers;

/// <summary>
/// Registro y consulta de usuarios.
/// </summary>
[ApiController]
[Route("api/users")]
[Produces("application/json")]
public class UsersController(IUserService service) : ControllerBase
{
    /// <summary>Registra un nuevo usuario.</summary>
    [HttpPost("register")]
    public async Task<ActionResult<UserResponse>> Register([FromBody] RegisterRequest request)
    {
        var created = await service.RegisterAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>
    /// Obtiene un usuario por su ID. Endpoint adicional a la consigna: lo usan
    /// Orders.API y Notifications.API para validar que el usuario existe.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<UserResponse>> GetById(Guid id)
    {
        return Ok(await service.GetByIdAsync(id));
    }
}

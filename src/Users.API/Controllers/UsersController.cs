using Users.API.Exceptions;
using Users.API.Swagger;
using Microsoft.AspNetCore.Mvc;
using Users.API.DTOs;
using Users.API.Services;

namespace Users.API.Controllers;

/// <summary>
/// Registro, login y consulta de usuarios.
/// </summary>
[ApiController]
[Route("api/users")]
[Produces("application/json")]
public class UsersController(IUserService service) : ControllerBase
{
    /// <summary>Registra un nuevo usuario.</summary>
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status201Created)]
    [ErrorResponse(400, ErrorCodes.USR_002, "Los datos del usuario son inválidos. El formato del email no es válido.")]
    [ErrorResponse(409, ErrorCodes.USR_001, "El email 'maria@email.com' ya está registrado.")]
    [ErrorResponse(500, ErrorCodes.USR_006, "Error interno al procesar el usuario.")]
    [HttpPost("register")]
    public async Task<ActionResult<UserResponse>> Register([FromBody] RegisterRequest request)
    {
        var created = await service.RegisterAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>Autentica a un usuario con email y contraseña.</summary>
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ErrorResponse(400, ErrorCodes.USR_002, "Los datos del usuario son inválidos. El email es obligatorio.")]
    [ErrorResponse(401, ErrorCodes.USR_003, "Credenciales incorrectas.")]
    [ErrorResponse(403, ErrorCodes.USR_004, "Su cuenta fue bloqueada por superar el máximo de intentos fallidos. Contacte a soporte.")]
    [ErrorResponse(403, ErrorCodes.USR_005, "Su cuenta fue suspendida por razones de seguridad. Contacte a soporte.")]
    [ErrorResponse(500, ErrorCodes.USR_006, "Error interno al procesar el usuario.")]
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
    {
        return Ok(await service.LoginAsync(request));
    }

    /// <summary>
    /// Obtiene un usuario por su ID. Endpoint adicional a la consigna: lo usan
    /// Orders.API y Notifications.API para validar que el usuario existe.
    /// </summary>
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ErrorResponse(404, ErrorCodes.USR_007, "Usuario no encontrado.")]
    [ErrorResponse(500, ErrorCodes.USR_006, "Error interno al procesar el usuario.")]
    [HttpGet("{id}")]
    public async Task<ActionResult<UserResponse>> GetById(Guid id)
    {
        return Ok(await service.GetByIdAsync(id));
    }
}

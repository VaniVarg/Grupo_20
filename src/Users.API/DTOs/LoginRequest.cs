using System.ComponentModel.DataAnnotations;

namespace Users.API.DTOs;

/// <summary>Cuerpo del POST /api/users/login.</summary>
public class LoginRequest
{
    /// <summary>Email registrado.</summary>
    /// <example>maria@email.com</example>
    [Required(ErrorMessage = "El email es obligatorio.")]
    public string Email { get; set; } = string.Empty;

    /// <summary>Contraseña del usuario.</summary>
    /// <example>MiPassword123!</example>
    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    public string Password { get; set; } = string.Empty;
}

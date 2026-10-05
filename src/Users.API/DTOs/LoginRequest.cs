using System.ComponentModel.DataAnnotations;

namespace Users.API.DTOs;

/// <summary>Cuerpo del POST /api/users/login.</summary>
public class LoginRequest
{
    [Required(ErrorMessage = "El email es obligatorio.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    public string Password { get; set; } = string.Empty;
}

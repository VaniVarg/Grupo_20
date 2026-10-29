namespace Users.API.DTOs;

/// <summary>
/// Usuario recién registrado. Nunca incluye PasswordHash.
/// </summary>
public class UserResponse
{
    /// <summary>Identificador único del usuario.</summary>
    /// <example>a1b2c3d4-0000-0000-0000-111122223333</example>
    public Guid Id { get; set; }
    /// <summary>Nombre del usuario.</summary>
    /// <example>María</example>
    public string Nombre { get; set; } = string.Empty;
    /// <summary>Apellido del usuario.</summary>
    /// <example>González</example>
    public string Apellido { get; set; } = string.Empty;
    /// <summary>Email del usuario.</summary>
    /// <example>maria@email.com</example>
    public string Email { get; set; } = string.Empty;
    /// <summary>Fecha de registro en UTC.</summary>
    /// <example>2026-10-28T12:00:00Z</example>
    public DateTime FechaRegistro { get; set; }
    /// <summary>False si el usuario está bloqueado.</summary>
    /// <example>true</example>
    public bool Activo { get; set; }
}

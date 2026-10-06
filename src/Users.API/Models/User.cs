namespace Users.API.Models;

/// <summary>
/// Usuario registrado en el sistema.
/// Regla de bloqueo: al llegar a 3 IntentosFallidos consecutivos, Activo pasa a false.
/// Si Activo es false con 3 o más intentos fallidos, el bloqueo fue automático (USR-004);
/// si Activo es false con menos de 3, fue un bloqueo manual por fraude (USR-005).
/// </summary>
public class User
{
    public Guid Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;

    /// <summary>Email único, con formato válido.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Hash de la contraseña. Nunca se incluye en ninguna respuesta de la API.</summary>
    public string PasswordHash { get; set; } = string.Empty;

    public DateTime FechaRegistro { get; set; }

    /// <summary>False cuando el usuario está bloqueado.</summary>
    public bool Activo { get; set; } = true;

    /// <summary>Se incrementa en cada login fallido y se resetea a 0 al loguearse bien.</summary>
    public int IntentosFallidos { get; set; }
}

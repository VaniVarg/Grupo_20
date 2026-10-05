namespace Users.API.Exceptions;

/// <summary>
/// Catálogo de códigos de error de Users.API.
/// </summary>
public static class ErrorCodes
{
    /// <summary>409 - El email ya está registrado.</summary>
    public const string USR_001 = "USR-001";

    /// <summary>400 - Datos del usuario inválidos.</summary>
    public const string USR_002 = "USR-002";

    /// <summary>401 - Credenciales incorrectas.</summary>
    public const string USR_003 = "USR-003";

    /// <summary>403 - Usuario bloqueado por demasiados intentos fallidos.</summary>
    public const string USR_004 = "USR-004";

    /// <summary>403 - Usuario bloqueado por detección de fraude.</summary>
    public const string USR_005 = "USR-005";

    /// <summary>500 - Error interno inesperado.</summary>
    public const string USR_006 = "USR-006";
}

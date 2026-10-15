namespace Notifications.API.Exceptions;

/// <summary>
/// Catálogo de códigos de error de Notifications.API.
/// </summary>
public static class ErrorCodes
{
	/// <summary>404 - El usuario destinatario no existe en Users.API.</summary>
	public const string NTF_001 = "NTF-001";

	/// <summary>400 - Datos de la notificación inválidos (campos faltantes o tipo no reconocido).</summary>
	public const string NTF_002 = "NTF-002";

	/// <summary>404 - El usuario no tiene notificaciones registradas.</summary>
	public const string NTF_003 = "NTF-003";

	/// <summary>500 - Error interno inesperado.</summary>
	public const string NTF_004 = "NTF-004";
}
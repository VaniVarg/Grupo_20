namespace Cart.API.Exceptions;

/// <summary>
/// Catálogo de códigos de error de Cart.API.
/// </summary>
public static class ErrorCodes
{
    /// <summary>404 - El usuario no tiene un carrito activo.</summary>
    public const string CRT_001 = "CRT-001";

    /// <summary>404 - El producto no existe en Products.API.</summary>
    public const string CRT_002 = "CRT-002";

    /// <summary>422 - La cantidad supera el stock disponible.</summary>
    public const string CRT_003 = "CRT-003";

    /// <summary>400 - Cantidad inválida (menor o igual a cero).</summary>
    public const string CRT_004 = "CRT-004";

    /// <summary>500 - Error interno inesperado.</summary>
    public const string CRT_005 = "CRT-005";
}
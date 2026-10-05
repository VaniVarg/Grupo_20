namespace Orders.API.Exceptions;

/// <summary>
/// Catálogo de códigos de error de Orders.API.
/// </summary>
public static class ErrorCodes
{
    /// <summary>404 - Orden no encontrada (GET/PUT con ID inexistente).</summary>
    public const string ORD_001 = "ORD-001";

    /// <summary>400 - Datos de la orden inválidos (campos faltantes o lista de items vacía).</summary>
    public const string ORD_002 = "ORD-002";

    /// <summary>404 - El usuario no existe en Users.API.</summary>
    public const string ORD_003 = "ORD-003";

    /// <summary>404 - Algún producto no existe en Products.API.</summary>
    public const string ORD_004 = "ORD-004";

    /// <summary>422 - Stock insuficiente para uno o más productos.</summary>
    public const string ORD_005 = "ORD-005";

    /// <summary>409 - Transición de estado no permitida.</summary>
    public const string ORD_006 = "ORD-006";

    /// <summary>500 - Error interno inesperado.</summary>
    public const string ORD_007 = "ORD-007";
}

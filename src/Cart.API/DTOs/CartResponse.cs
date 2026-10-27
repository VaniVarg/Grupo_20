namespace Cart.API.DTOs;

/// <summary>
/// Carrito de un usuario tal como lo devuelve la API.
/// </summary>
public class CartResponse
{
    /// <summary>Usuario dueño del carrito.</summary>
    /// <example>a1b2c3d4-0000-0000-0000-111122223333</example>
    public Guid UsuarioId { get; set; }
    public List<CartItemResponse> Items { get; set; } = [];
    /// <summary>Última modificación en UTC.</summary>
    /// <example>2026-10-28T12:00:00Z</example>
    public DateTime FechaActualizacion { get; set; }
}
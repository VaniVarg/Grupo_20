namespace Cart.API.Models;

/// <summary>
/// Carrito de compras de un usuario. Cada usuario tiene como máximo un carrito.
/// La clase se llama ShoppingCart (y no Cart) porque el namespace raíz del proyecto
/// ya es "Cart" y el compilador se confundiría entre el namespace y la clase.
/// </summary>
public class ShoppingCart
{
    /// <summary>Usuario dueño del carrito (vive en Users.API). Es la clave del carrito.</summary>
    public Guid UsuarioId { get; set; }

    public List<CartItem> Items { get; set; } = [];

    /// <summary>Se actualiza automáticamente en cada operación sobre el carrito.</summary>
    public DateTime FechaActualizacion { get; set; }
}
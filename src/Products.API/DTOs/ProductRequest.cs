using System.ComponentModel.DataAnnotations;

namespace Products.API.DTOs;

/// <summary>
/// Cuerpo de las peticiones POST y PUT de productos.
/// </summary>
public class ProductRequest
{
    /// <summary>Nombre del producto.</summary>
    /// <example>Notebook Dell XPS 15</example>
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    /// <summary>Descripción opcional.</summary>
    /// <example>Laptop 15 pulgadas, 32GB RAM</example>
    [StringLength(500, ErrorMessage = "La descripción no puede superar los 500 caracteres.")]
    public string? Descripcion { get; set; }

    /// <summary>Precio unitario (mayor a 0).</summary>
    /// <example>1500.00</example>
    [Range(0.01, 999999999, ErrorMessage = "El precio debe ser mayor a 0.")]
    public decimal Precio { get; set; }

    /// <summary>Stock disponible (mayor o igual a 0).</summary>
    /// <example>10</example>
    [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo.")]
    public int Stock { get; set; }

    /// <summary>Categoría del producto.</summary>
    /// <example>Electrónica</example>
    [Required(ErrorMessage = "La categoría es obligatoria.")]
    public string Categoria { get; set; } = string.Empty;
}
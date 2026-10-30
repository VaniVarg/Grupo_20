using System.ComponentModel.DataAnnotations;

namespace Products.API.DTOs;

/// Cuerpo de las peticiones POST y PUT de productos.
public class ProductRequest
{
    //Nombre del producto.<example>Notebook Dell XPS 15</example>
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    //Descripción opcional.<example>Laptop 15 pulgadas, 32GB RAM</example>
    [StringLength(500, ErrorMessage = "La descripción no puede superar los 500 caracteres.")]
    public string? Descripcion { get; set; }

    //Precio unitario (mayor a 0).<example>1500.00</example>
    [Range(0.01, 999999999, ErrorMessage = "El precio debe ser mayor a 0.")]
    public decimal Precio { get; set; }

    //Stock disponible (mayor o igual a 0) <example>10</example>
    [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo.")]
    public int Stock { get; set; }

    //Categoría del producto.<example>Electrónica</example>
    [Required(ErrorMessage = "La categoría es obligatoria.")]
    public string Categoria { get; set; } = string.Empty;
}
